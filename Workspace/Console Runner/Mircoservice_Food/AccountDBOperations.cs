using System.Security.Cryptography;
using System.Text;

using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public class AccountDBOperations
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountGateway _accountAccess;
        private readonly IAuthorizationGateway _permissionService;
        private readonly IFlagGateway _flagService;
        private readonly IAMRGateway _amrGateway;
        private readonly IActiveSessionTrackerGateway _activeSessionTrackerGateway;

        public AccountDBOperations(IAccountGateway accountAccess, IAuthorizationGateway permissionService, IFlagGateway flagGateway, IAMRGateway aMRGateway, IActiveSessionTrackerGateway activeSessionTrackerGateway)
        {
            this._accountAccess = accountAccess;
            this._permissionService = permissionService;
            this._flagService = flagGateway;
            this._amrGateway = aMRGateway;
            this._activeSessionTrackerGateway = activeSessionTrackerGateway;
        }
        
        /// <summary>
        /// Computes the hash from provided information
        /// </summary>
        /// <param name="bytesToHash"></param>
        /// <param name="salt"></param>
        /// <param name="logService"></param>
        /// <returns>a string representing the computed hash</returns>
        private string ComputeHash(byte[] bytesToHash, byte[] salt, LogService? logService = null)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        /// <summary>
        /// Generates a Salt using a cryptographic random number generator 
        /// </summary>
        /// <param name="logService"></param>
        /// <returns></returns>
        private string GenerateSalt(LogService? logService = null)
        {
            var bytes = new byte[128 / 8];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Takes in user information and persists that data to allow for future logins
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="logService"></param>
        /// <returns>true if login successful, false otherwise</returns>
        public async Task<bool> UserSignUpAsync(Account acc, LogService? logService = null)
        {
            try
            {
                string salt = GenerateSalt();
                acc.salt = salt;
                byte[] saltBytes = Encoding.ASCII.GetBytes(acc.salt);
                byte[] passBytes = Encoding.ASCII.GetBytes(acc.Password);
                acc.Password = ComputeHash(saltBytes, passBytes);

                //Need to validate users dont have duplicate Emails.
                acc.IsActive = false;
                await _accountAccess.AddAccountAsync(acc, logService);
                await _permissionService.AssignDefaultUserPermissions(acc.UserID, logService);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User {acc.Email} successfully signed up.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {acc.Email} could not be signed up. Unknown error: {ex.Message}");
                }
                throw;
            }

        }

            /// <summary>
            /// Deletes a user from the DB
            /// </summary>
            /// <param name="currentUser"></param>
            /// <param name="userID"></param>
            /// <param name="logService"></param>
            /// <returns>true if successful false otherwise</returns>
            /// <exception cref="InvalidOperationException"></exception>
            /// <exception cref="UserNotAuthorizedException"></exception>
        public async Task<bool> UserDeleteAsync(Account currentUser, int userID, LogService? logService = null)
        {
            try
            {
                // cancel if the account to delete does not exist
                if (! await _accountAccess.AccountExistsAsync(userID, logService))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be deleted. Account does not exist.");
                    }
                    throw new InvalidOperationException("Account to be deleted does not exist.");
                }
                Account? acc = await _accountAccess.GetAccountAsync(userID, logService);
                // cancel if the current user does not have permission to delete this account
                if (!await _permissionService.HasPermissionAsync(currentUser.UserID, "createAdmin", logService))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be deleted. User {currentUser.UserID} does not have authorization to delete.");
                    }
                    throw new UserNotAuthorizedException("User has insufficient permission.");
                }
                // cancel if this results in no admins
                if(currentUser.UserID == userID &&  (_permissionService.AdminCount(logService) == 1))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be deleted. Deletion will result in no admins remaining.");
                    }
                    throw new InvalidOperationException("This operation will result in no admins and can not be completed.");
                }
                // checks done, proceed with deletion
                _permissionService.RemoveAllUserPermissions(acc.UserID, logService);
                await _accountAccess.RemoveAccountAsync(acc, logService);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"Deleted user {userID}.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be deleted. Unknown error: {ex.Message}");
                }
                throw;
            }
        }

        /// <summary>
        /// Retrieve an Account object from the database.
        /// </summary>
        /// <param name="UserID">UserID  to retrieve</param>
        /// <returns>Account object with the provided AccountID assuming it exists, otherwise null if the account does not exist.</returns>
        public async Task<Account?> GetUserAccountAsync(int userID, LogService? logService = null)
        {
            try
            {
                Account? toReturn = await _accountAccess.GetAccountAsync(userID, logService);
                if (logService?.UserID != null)
                {
                    if (toReturn != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                            $"Retrieved user {userID} from the database.");
                    }
                    else
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                            $"Could not retrieve user {userID} from the database. User does not exist.");
                    }
                }
                return toReturn;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                       $"Could not retrieve user {userID}. Unknown error: {ex.Message}");
                }
                throw new Exception("Unknown error");
            }
        }

        /// <summary>
        /// Update an Account object in the database. Modify the account object, then pass it into this method. The corresponding object in the database will be updated accordingly.
        /// </summary>
        /// <param name="acc">The Account object with modified parameters</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public async Task<bool> UserUpdateDataAsync(Account currentUser, int userID, string nFname, string nLname, string npassword, LogService? logService = null)
        {
            bool fNameChanged = false, lNameChanged = false, passwordChanged = false;
            string fTemp = "", lTemp = "", pTemp = "";
            if (currentUser.UserID != userID)
            {
                if ((! await _permissionService.HasPermissionAsync(currentUser.UserID, "editOtherAccount", logService)) || (!currentUser.IsActive))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be updated. User {currentUser.UserID} does not have authorization to update.");
                    }
                    throw new UserNotAuthorizedException($"User {currentUser.UserID} not authorized to update user {userID}");
                }
            }
            try
            {
                Account? acc = await _accountAccess.GetAccountAsync(userID, logService);
                if (acc == null)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be updated. User does not exist.");
                    }
                    throw new ArgumentException($"User {userID} does not exist");
                }
                if (nFname != "")
                {
                    fTemp = acc.FName;
                    acc.FName = nFname;
                    fNameChanged = true;
                }
                if (nLname != "")
                {
                    lTemp = acc.LName;
                    acc.LName = nLname;
                    lNameChanged = true;
                }
                if (npassword != "")
                {
                    pTemp = acc.Password;
                    acc.Password = npassword;
                    passwordChanged = true;
                }
                acc.IsActive = false;
                await _accountAccess.UpdateAccountAsync(acc);

                /*if (fNameChanged)
                    //_logger.LogAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, fTemp, nFname);
                if (lNameChanged)
                   // _logger.LogAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, lTemp, nLname);
                if (passwordChanged)
                   // _logger.LogAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, pTemp, npassword);*/
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User {userID} successfully updated.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be updated. Unknown error: {ex.Message}");
                }
                throw;
            }
        }


        /// <summary>
        /// Takes in a users information and validates their credentials 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userPass"></param>
        /// <param name="logService"></param>
        /// <returns>true if the password matches false if it does not</returns>
        public async Task<bool> AuthenticateUserPassAsync(string email, string userPass, LogService? logService = null)
        {
            int userID = await _accountAccess.GetIDFromEmailIdAsync(email, logService);
            if (userID == -1)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"Authentication for email address {email} failed. Email address does not exist.");
                }
                return false;
            }
            string salt = _accountAccess.getSalt(userID, logService);
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
            byte[] passBytes = Encoding.ASCII.GetBytes(userPass);
            string hashedPass = ComputeHash(saltBytes, passBytes);
            Account? acc = await GetUserAccountAsync(userID);
            bool toReturn = (acc != null && acc.Password == hashedPass);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                   toReturn ? $"Authentication successful for email address {email}." :
                                                   $"Authentication for email address {email} failed. Password does not match.");
            }
            return toReturn;
        }

        /// <summary>
        /// Takes the users information to log them into the website
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userPass"></param>
        /// <param name="logService"></param>
        /// <returns>an instance of the users account object if the operation was successful, null otherwise</returns>
        public async Task<Account> SignInAsync(string email, string userPass, LogService? logService = null)
        {
            
            if (await AuthenticateUserPassAsync(email, userPass))
            {
                int ID = await _accountAccess.GetIDFromEmailIdAsync(email, logService);
                Account acc = await GetUserAccountAsync(ID);
                acc.IsActive = true;
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                                       $"User {acc.UserID} successfully signed in.");
                }
                return acc;
            }
            else
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                                       $"User failed to sign in. Authentication failed.");
                }
                return null;
            }
        }

        /// <summary>
        /// Disables the account with userID of targetPK if exists
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns>true if the operation was successful, false otherwise</returns>
        /// <exception cref="UserNotAuthorizedException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> DisableAccountAsync(Account currentUser, int userID, LogService? logService = null)
        {
            // cancel if acting user does not have permission to disable
            if (! await _permissionService.HasPermissionAsync(currentUser.UserID, "disableAccount", logService) || !currentUser.IsActive)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be disabled. User {currentUser.UserID} does not have authorization to disable.");
                }
                throw new UserNotAuthorizedException("The account does not have required permissions to disable another account");
            }
            // cancel if target user does not exist
            if (! await _accountAccess.AccountExistsAsync(userID, logService))
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be disabled. User {userID} does not exist.");
                }
                throw new ArgumentException("The account being requested for deletion does not exist");
            }
            try
            {
                Account? acc = await _accountAccess.GetAccountAsync(userID, logService);
                if (_permissionService.IsAdmin(currentUser.UserID, logService) && (_permissionService.AdminCount(logService) > 1))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be disabled. Disabling this user will result in no active admins.");
                    }
                    throw new ArgumentException("Disabling this account would result in there being no admins.");
                }
                acc.Enabled = false;
                acc.IsActive = false;
                await _accountAccess.UpdateAccountAsync(acc, logService);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} successfully disabled.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be disabled. Unknown error: {ex.Message}");
                }
                throw;
            }
        }
        /// <summary>
        /// enables the target users account
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns>true if successful</returns>
        public async Task<bool> EnableAccountAsync(Account currentUser, int userID, LogService? logService = null)
        {
            // cancel if acting user does not have permission
            if (! await _permissionService.HasPermissionAsync(currentUser.UserID, "enableAccount", logService) || !currentUser.IsActive)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be enabled. User {userID} does not exist.");
                }
                return false;
            }
            try
            {
                if (! await _accountAccess.AccountExistsAsync(userID, logService))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be enabled. User {currentUser.UserID} does not have authorization to enable.");
                    }
                    return false;
                }
                Account? acc = await _accountAccess.GetAccountAsync(userID, logService);
                acc.Enabled = true;
                await _accountAccess.UpdateAccountAsync(acc, logService);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} successfully enabled.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be enabled. Unknown error: {ex.Message}");
                }
                throw;
            }
        }

        /// <summary>
        /// Checks if an account exists based on userID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns>true if the user exists false if they do not</returns>
        public async Task<bool> AccountExistsAsync(int userID, LogService? logService = null)
        {
            bool toReturn = await _accountAccess.AccountExistsAsync(userID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Checked whether {userID} exists ({toReturn})");
            }
            return toReturn;
        }

        /// <summary>
        /// Promotes a user to admin
        /// </summary>
        /// <param name="currentUser">The user attempting to do the operation</param>
        /// <param name="userID">the targeted user</param>
        /// <param name="logService"></param>
        /// <returns>true if successful false otherwise</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> PromoteToAdmin(Account currentUser, int userID, LogService? logService = null)
        {
            try
            {
                if (await _permissionService.HasPermissionAsync(currentUser.UserID, "createAdmin", logService) && currentUser.IsActive)
                {
                    Account? acc = await _accountAccess.GetAccountAsync(userID, logService);
                    if (acc == null)
                    {
                        if (logService?.UserID != null)
                        {
                            _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                               $"User {userID} could not be promoted to admin. User {userID} does not exist.");
                        }
                        throw new ArgumentException("The requested account does not exist");
                        //return false;
                    }
                    await _permissionService.AssignDefaultAdminPermissions(acc.UserID, logService);
                    await _accountAccess.UpdateAccountAsync(acc, logService);
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                                           $"User {userID} successfully promoted to admin.");
                    }
                    return true;
                }
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be promoted to admin. User {currentUser.UserID} does not have authorization to enable.");
                }
                return false;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be enabled. Unknown error: {ex.Message}");
                }
                throw;
            }
        }
        /// <summary>
        /// Adds a permission to the specified user
        /// </summary>
        /// <param name="currentUser">The user preforming this operation</param>
        /// <param name="userID">Target user of this operation</param>
        /// <param name="PermissionToBeAdded"></param>
        /// <param name="logService"></param>
        /// <returns>true if successful false otherwise</returns>
        public async Task<bool> addPermissionAsync(Account currentUser, int userID, string PermissionToBeAdded, LogService? logService = null)
        {
            if(IsAdmin(currentUser.UserID))
            {
                if(await HasPermissionAsync(currentUser.UserID, PermissionToBeAdded, logService))
                {
                    Authorization newPerm = new(PermissionToBeAdded);
                    newPerm.UserID = userID;
                    await _permissionService.AddPermissionAsync(newPerm, logService);
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                           $"Successfully added permission for user {userID} and resource {PermissionToBeAdded}.");
                    }
                    return true;
                }
                else
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"Cannot add permission for user {userID} and resource {PermissionToBeAdded}. " +
                                                           $"User {currentUser.UserID} does not have the permission to be granted.");
                    }
                }
            }
            else
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"Cannot add permission for user {userID} and resource {PermissionToBeAdded}. " +
                                                       $"User {currentUser.UserID} is not authorized to grant permissions.");
                }
            }

            return false;
        }
        
        /// <summary>
        /// Checks if a user has a specific permission
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="permission"></param>
        /// <param name="logService"></param>
        /// <returns>True if they do false if they do not</returns>
        public async Task<bool> HasPermissionAsync(int userID, string permission, LogService? logService = null)
        {
            bool toReturn = await _permissionService.HasPermissionAsync(userID, permission, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Checked if user {userID} has permission for resource {permission} ({toReturn}).");
            }
            return toReturn;
        }

        /// <summary>
        /// Checks if an account has admin access
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns>true if the user is an admin false if they are not</returns>
        public bool IsAdmin(int userID, LogService? logService = null)
        {
            bool toReturn = _permissionService.IsAdmin(userID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Checked if user {userID} is admin ({toReturn}).");
            }
            return toReturn;
        }

        /// <summary>
        /// Adds a Food Flag to the users account
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="IngredientID"></param>
        /// <param name="logService"></param>
        /// <returns>true if the opperation is successful, false otherwise</returns>
        public async Task<bool> AddFlagToAccountAsync(int userID, int IngredientID, LogService? logService = null)
        {
            FoodFlag foodFlag = new(userID, IngredientID);
            bool toReturn = await _flagService.AddFlagAsync(foodFlag, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                   $"Successfully created food flag between user {userID} and ingredient {IngredientID}.");
            }
            return toReturn;
        }
        /// <summary>
        /// Removes a food flag from a users account
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="IngredientID"></param>
        /// <param name="logService"></param>
        /// <returns>true if the opperation is successful, false otherwise</returns>
        public async Task<bool> RemoveFoodFlagAsync(int userID, int IngredientID, LogService? logService = null)
        {
            bool toReturn = await _flagService.RemoveFoodFlagAsync(userID, IngredientID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Successfully removed food flag between user {userID} and ingredient {IngredientID}.");
            }
            return toReturn;
        }
        /// <summary>
        /// Checks if an account has a specific ingredient as a food flag
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="IngredientID"></param>
        /// <param name="logService"></param>
        /// <returns>true if the opperation is successful, false otherwise</returns>
        public async Task<bool> accountHasFlagAsync(int userID, int IngredientID, LogService? logService = null)
        {
            bool toReturn = await _flagService.AccountHasFlagAsync(userID, IngredientID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Checked for food flag between user {userID} and ingredient {IngredientID} ({toReturn}).");
            }
            return toReturn;
        }
        /// <summary>
        /// Gets N(take) flags that belong to the userID provided while skipping over first m(skip) results. 
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved</param>
        /// <param name="skip">The number of entries to skip before pulling</param>
        /// <param name="take">The number of entries to return</param>
        /// <returns>A list containing the food flags associated with the user</returns>
        public async Task<List<FoodFlag>> GetNAccountFlagsAsync(int userID, int skip, int take, LogService? logService = null)
        {
            return await _flagService.GetNAccountFlagsAsync(userID, skip, take, logService);
        }

        public async Task<bool> AddAMRAsync(int userID, bool isMale, int weight, float height, int age, ActivityLevel activity)
        {
            await _amrGateway.AddAMRAsync(new AMR(isMale, weight, height, age, activity) { UserID = userID });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved</param>
        /// <returns>A list of all flags associated with the users account</returns>
        public async Task<List<FoodFlag>> GetAllAccountFlagsAsync(int userID, LogService? logService = null)
        {
            return await _flagService.GetAllAccountFlagsAsync(userID, logService);
        }

        public async Task<AMR> GetAMRAsync(int userID, LogService? logService = null)
        {
            return await _amrGateway.GetAMRAsync(userID, logService);
        }

        public async Task<bool> StartSessionAsync(int userID, string jwt)
        {
            return await _activeSessionTrackerGateway.StartSessionAsync(userID, jwt);
        }

        public async Task<int> GetActiveUserAsync(string jwt)
        {
            return await _activeSessionTrackerGateway.GetActiveUserAsync(jwt);
        }

        public async Task<bool> ValidateToken(string jwt)
        {
            return await _activeSessionTrackerGateway.ValidateToken(jwt);
        }
    }




    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException()
        {

        }
        public UserNotAuthorizedException(string message) : base(message)
        {

        }
        public UserNotAuthorizedException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
