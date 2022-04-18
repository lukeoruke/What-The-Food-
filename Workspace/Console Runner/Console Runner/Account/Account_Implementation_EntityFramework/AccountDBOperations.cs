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
        public AccountDBOperations(IAccountGateway accountAccess, IAuthorizationGateway permissionService, IFlagGateway flagGateway)
        {
            this._accountAccess = accountAccess;
            this._permissionService = permissionService;
            this._flagService = flagGateway;
        }

        private string ComputeHash(byte[] bytesToHash, byte[] salt, LogService? logService = null)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        private string GenerateSalt(LogService? logService = null)
        {
            var bytes = new byte[128 / 8];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

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
                    _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User {acc.Email} successfully signed up.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {acc.Email} could not be signed up. Unknown error: {ex.Message}");
                }
                throw;
            }

        }

        /*
		 * Delets a user corresponding to the email provided as arg
		 * Takes in currentUser to validate user calling method has permission to do so
		 */
        public async Task<bool> UserDeleteAsync(Account currentUser, int userID, LogService? logService = null)
        {
            try
            {
                // cancel if the account to delete does not exist
                if (! await _accountAccess.AccountExistsAsync(userID, logService))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Error, Category.Business, DateTime.Now,
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
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be deleted. User {currentUser.UserID} does not have authorization to delete.");
                    }
                    throw new UserNotAuthorizedException("User has insufficient permission.");
                }
                // cancel if this results in no admins
                if(currentUser.UserID == userID &&  (_permissionService.AdminCount(logService) == 1))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be deleted. Deletion will result in no admins remaining.");
                    }
                    throw new InvalidOperationException("This operation will result in no admins and can not be completed.");
                }
                // checks done, proceed with deletion
                _permissionService.RemoveAllUserPermissions(acc.UserID, logService);
                await _accountAccess.RemoveAccountAsync(acc, logService);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"Deleted user {userID}.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be deleted. Unknown error: {ex.Message}");
                }
                throw;
            }
        }

        //will return an account object from the DB given a PK from the argument field
        public async Task<Account?> GetUserAccountAsync(int userID, LogService? logService = null)
        {
            try
            {
                Account? toReturn = await _accountAccess.GetAccountAsync(userID, logService);
                if (logService?.UserID != null)
                {
                    if (toReturn != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                            $"Retrieved user {userID} from the database.");
                    }
                    else
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                            $"Could not retrieve user {userID} from the database. User does not exist.");
                    }
                }
                return toReturn;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Error, Category.Business, DateTime.Now,
                                                       $"Could not retrieve user {userID}. Unknown error: {ex.Message}");
                }
                throw new Exception("Unknown error");
            }
        }

        //will update a user's data from a given PK in the argument, fields being changed are given in the argument line as well, null input means no change
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
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
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
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
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
                    _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User {userID} successfully updated.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be updated. Unknown error: {ex.Message}");
                }
                throw;
            }
        }


        //authenticates a users input password for login. True if pass matches, false otherwise
        public async Task<bool> AuthenticateUserPassAsync(string email, string userPass, LogService? logService = null)
        {
            int userID = await _accountAccess.GetIDFromEmail(email, logService);
            if (userID == -1)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
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
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                   toReturn ? $"Authentication successful for email address {email}." :
                                                   $"Authentication for email address {email} failed. Password does not match.");
            }
            return toReturn;
        }

        //takes in username and password. If valid returns an account object for the user with specified data.
        public async Task<Account> SignInAsync(string email, string userPass, LogService? logService = null)
        {
            
            if (await AuthenticateUserPassAsync(email, userPass))
            {
                int ID = await _accountAccess.GetIDFromEmail(email, logService);
                Account acc = await GetUserAccountAsync(ID);
                acc.IsActive = true;
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Business, DateTime.Now,
                                                       $"User {acc.UserID} successfully signed in.");
                }
                return acc;
            }
            else
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Business, DateTime.Now,
                                                       $"User failed to sign in. Authentication failed.");
                }
                return null;
            }
        }

        /*
		 * Disables the account with email of targetPK if exists
		 * currentUser is taken in to validate the user calling this has permission to do so
		 */
        public async Task<bool> DisableAccountAsync(Account currentUser, int userID, LogService? logService = null)
        {
            // cancel if acting user does not have permission to disable
            if (! await _permissionService.HasPermissionAsync(currentUser.UserID, "disableAccount", logService) || !currentUser.IsActive)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be disabled. User {currentUser.UserID} does not have authorization to disable.");
                }
                throw new UserNotAuthorizedException("The account does not have required permissions to disable another account");
            }
            // cancel if target user does not exist
            if (! await _accountAccess.AccountExistsAsync(userID, logService))
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be disabled. User {userID} does not exist.");
                }
                throw new ArgumentException("The account being requested for deletion does not exist");
            }
            try
            {
                Account? acc = await _accountAccess.GetAccountAsync(userID, logService);
                // acc is not null because we already checked above
                acc = null!;
                if (_permissionService.IsAdmin(currentUser.UserID, logService) && (_permissionService.AdminCount(logService) > 1))
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be disabled. Disabling this user will result in no active admins.");
                    }
                    throw new ArgumentException("Disabling this account would result in there being no admins.");
                }
                acc.Enabled = false;
                acc.IsActive = false;
                await _accountAccess.UpdateAccountAsync(acc, logService);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} successfully disabled.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be disabled. Unknown error: {ex.Message}");
                }
                throw;
            }
        }
        /*
		 * Enables the targeted account. 
		 * currentUser is used to validate that the person calling this method has permission to do so.
		 * targetPK is the email of the user whos account is being activated
		 */
        public async Task<bool> EnableAccountAsync(Account currentUser, int userID, LogService? logService = null)
        {
            // cancel if acting user does not have permission
            if (! await _permissionService.HasPermissionAsync(currentUser.UserID, "enableAccount", logService) || !currentUser.IsActive)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
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
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"User {userID} could not be enabled. User {currentUser.UserID} does not have authorization to enable.");
                    }
                    return false;
                }
                Account? acc = await _accountAccess.GetAccountAsync(userID, logService);
                // acc is not null because of the check above
                acc = null!;
                acc.Enabled = true;
                await _accountAccess.UpdateAccountAsync(acc, logService);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} successfully enabled.");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Error, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be enabled. Unknown error: {ex.Message}");
                }
                throw;
            }
        }

        public async Task<bool> AccountExistsAsync(int userID, LogService? logService = null)
        {
            bool toReturn = await _accountAccess.AccountExistsAsync(userID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Checked whether {userID} exists ({toReturn})");
            }
            return toReturn;
        }

        /*promotes the target user to admin
		 * takes in currentUser to verify the current session is being handled by an admin
		 * targetPK is the email(primary key) of the user being targeted
		 */
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
                            _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                               $"User {userID} could not be promoted to admin. User {userID} does not exist.");
                        }
                        throw new ArgumentException("The requested account does not exist");
                        //return false;
                    }
                    await _permissionService.AssignDefaultAdminPermissions(acc.UserID, logService);
                    await _accountAccess.UpdateAccountAsync(acc, logService);
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Business, DateTime.Now,
                                                           $"User {userID} successfully promoted to admin.");
                    }
                    return true;
                }
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be promoted to admin. User {currentUser.UserID} does not have authorization to enable.");
                }
                return false;
            }
            catch (Exception ex)
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User {userID} could not be enabled. Unknown error: {ex.Message}");
                }
                throw;
            }
        }
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
                        _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                           $"Successfully added permission for user {userID} and resource {PermissionToBeAdded}.");
                    }
                    return true;
                }
                else
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                           $"Cannot add permission for user {userID} and resource {PermissionToBeAdded}. " +
                                                           $"User {currentUser.UserID} does not have the permission to be granted.");
                    }
                }
            }
            else
            {
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"Cannot add permission for user {userID} and resource {PermissionToBeAdded}. " +
                                                       $"User {currentUser.UserID} is not authorized to grant permissions.");
                }
            }

            return false;
        }
        
        public async Task<bool> HasPermissionAsync(int userID, string permission, LogService? logService = null)
        {
            bool toReturn = await _permissionService.HasPermissionAsync(userID, permission, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Checked if user {userID} has permission for resource {permission} ({toReturn}).");
            }
            return toReturn;
        }

        public bool IsAdmin(int userID, LogService? logService = null)
        {
            bool toReturn = _permissionService.IsAdmin(userID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Checked if user {userID} is admin ({toReturn}).");
            }
            return toReturn;
        }

        public async Task<bool> AddFlagToAccountAsync(int userID, int IngredientID, LogService? logService = null)
        {
            FoodFlag foodFlag = new(userID, IngredientID);
            bool toReturn = await _flagService.AddFlagAsync(foodFlag, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Warning, Category.Business, DateTime.Now,
                                                   $"Successfully created food flag between user {userID} and ingredient {IngredientID}.");
            }
            return toReturn;
        }

        public async Task<bool> RemoveFoodFlagAsync(int userID, int IngredientID, LogService? logService = null)
        {
            bool toReturn = await _flagService.RemoveFoodFlagAsync(userID, IngredientID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Successfully removed food flag between user {userID} and ingredient {IngredientID}.");
            }
            return toReturn;
        }

        public async Task<bool> accountHasFlagAsync(int userID, int IngredientID, LogService? logService = null)
        {
            bool toReturn = await _flagService.AccountHasFlagAsync(userID, IngredientID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Checked for food flag between user {userID} and ingredient {IngredientID} ({toReturn}).");
            }
            return toReturn;
        }

        public List<FoodFlag> GetAllAccountFlags(int userID, LogService? logService = null)
        {
            List<FoodFlag> toReturn = _flagService.GetAllAccountFlags(userID, logService);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Business, DateTime.Now,
                                                   $"Successfully retrieved all food flags for user {userID}.");
            }
            return toReturn;
        }



        /////////////////////////////////////////////////////////////////TODO THIS IS ON HOLD UNTIL SERVICE MANAGER IS COMPLETED.////////////////////////////////////////////////////////////////



        /*        public List<Ingredient> CheckProductForFlags(string barcode, string email)
                {
                    FoodItem? food = GetScannedFoodItem(barcode);
                    if (food == null) return new List<Ingredient>();
                    List<Ingredient> ingredientList = GetIngredientList(food.Barcode);
                    List<Ingredient> flaggedItems = new List<Ingredient>();
                    for (int i = 0; i < ingredientList.Count; i++)
                    {
                        if (_flagGateway.AccountHasFlag(email, ingredientList[i].IngredientID))
                        {
                            flaggedItems.Add(ingredientList[i]);
                        }
                    }
                    return flaggedItems;
                }*/


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
