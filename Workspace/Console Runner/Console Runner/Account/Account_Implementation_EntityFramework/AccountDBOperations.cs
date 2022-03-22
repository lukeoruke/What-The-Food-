

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

        
        public async Task<bool> UserSignUpAsync(Account acc)
        {
            try
            {
                if(_accountAccess.GetIDFromEmail(acc.Email) == -1); //TODO Validate an ID of 0 will be returned if the user does not exist.
               
                {
                    Console.WriteLine("email already in use");
                    return false;
                }
                await _permissionService.AssignDefaultUserPermissions(acc.UserID);
                acc.IsActive = false;
                await _accountAccess.AddAccountAsync(acc);
                //_logger.LogAccountCreation(UM_CATEGORY, "Signup page", true, "", acc.Email);
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //_logger.LogAccountCreation(UM_CATEGORY, "test page", false, ex.Message, acc.Email);
                return false;
            }

        }

        /*
		 * Delets a user corosponding to the email provided as arg
		 * Takes in currentUser to validate user calling method has permission to do so
		 */
        public async Task<bool> UserDeleteAsync(Account currentUser, int userID)
        {

         /*   if (!_permissionService.HasPermission(currentUser.Email, "deleteAccount"))
            {
                _logger.LogAccountDeletion(UM_CATEGORY, "test page", false, "ADMIN ACCESS NEEDED", currentUser.Email);
                return false;
            }*/
            try
            {
                if (! await _accountAccess.AccountExistsAsync(userID))
                {
                    return false;
                }

                Account? acc = await _accountAccess.GetAccountAsync(userID);

                if (await _permissionService.HasPermissionAsync(userID, "createAdmin") && (_permissionService.AdminCount() < 2))
                {
                    Console.WriteLine("Deleting this account would result in there being no admins.");
                    return false;
                }

                // _permissionService.RemoveAllUserPermissions(acc.Email);
                await _accountAccess.RemoveAccountAsync(acc);
               // _logger.LogAccountDeletion(UM_CATEGORY, "test page", true, "", acc.Email);


                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogAccountDeletion(UM_CATEGORY, "test page", false, ex.Message, targetEmail);
                return false;
            }
        }

        //will return an account object from the DB given a PK from the argument field
        public async Task<Account> GetUserAccountAsync(int userID)
        {
            try
            {

                if (await _accountAccess.AccountExistsAsync(userID))
                {
                    return await _accountAccess.GetAccountAsync(userID);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogGeneric(UM_CATEGORY, "test page", false, ex.Message, targetPK, "Failed to read.");
                return null;
            }
        }

        //will update a user's data from a given PK in the argument, fields being changed are given in the argument line as well, null input means no change
        public async Task<bool> UserUpdateDataAsync(Account currentUser, int userID, string nFname, string nLname, string npassword)
        {
            bool fNameChanged = false, lNameChanged = false, passwordChanged = false;
            string fTemp = "", lTemp = "", pTemp = "";
            if (currentUser.UserID != userID)
            {
                if (! await _permissionService.HasPermissionAsync(currentUser.UserID, "editOtherAccount") || !currentUser.IsActive)
                {
                    //_logger.LogGeneric(UM_CATEGORY, "test page", false, "ADMIN ACCESS NEEDED", currentUser.Email, "ADMIN ACCESS NEEDED TO UPDATE USER DATA");
                    return false;
                }
            }
            try
            {
                Account? acc = await _accountAccess.GetAccountAsync(userID);
                if (acc == null)
                {
                    Console.WriteLine("NULL ACCOUNT FOUND");
                    return false;
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

                await _accountAccess.UpdateAccountAsync(acc);

                if (fNameChanged)
                    //_logger.LogAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, fTemp, nFname);
                if (lNameChanged)
                   // _logger.LogAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, lTemp, nLname);
                if (passwordChanged)
                   // _logger.LogAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, pTemp, npassword);

                acc.IsActive = false;
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
               // _logger.LogGeneric(UM_CATEGORY, "test page", false, ex.Message, targetPK, "Could not change user info");
                return false;
            }
        }


        //authenticates a users input password for login. True if pass matches, false otherwise
        public async Task<bool> AuthenticateUserPassAsync(int userID, string userPass)
        {
            Account acc = await GetUserAccountAsync(userID);
            return (acc != null && acc.Password == userPass);
        }
        //takes in username and password. If valid returns an account object for the user with specified data.
        public async Task<Account> SignIn(string email, string userPass)
        {
            int ID = _accountAccess.GetIDFromEmail(email);
            if (await AuthenticateUserPassAsync(ID, userPass))
            {
              //  _logger.LogLogin(UM_CATEGORY, "test page", true, "", user);
                Account acc = await GetUserAccountAsync(ID);
                acc.IsActive = true;
                return acc;
            }
            else
            {
                //_logger.LogLogin(UM_CATEGORY, "test page", false, "Invalid Password", user);
                return null;
            }
        }

        /*
		 * Disables the account with email of targetPK if exists
		 * currentUser is taken in to validate the user calling this has permission to do so
		 */
        public async Task<bool> DisableAccountAsync(Account currentUser, int userID)
        {
            if (! await _permissionService.HasPermissionAsync(currentUser.UserID, "disableAccount") || !currentUser.IsActive)
            {
                //_logger.LogAccountDeactivation(UM_CATEGORY, "Console", false, "ADMIN ACCESS NEEDED", currentUser.Email, "No Target");
                return false;
            }
            if (! await _accountAccess.AccountExistsAsync(userID))
            {
                return false;
            }
            try
            {
                Account? acc = await _accountAccess.GetAccountAsync(userID);

                if (_permissionService.IsAdmin(userID) && (_permissionService.AdminCount() < 2))
                {
                    //Console.WriteLine("Disabling this account would result in there being no admins.");
                    return false;
                }
                acc.Enabled = false;
                acc.IsActive = false;
                await _accountAccess.UpdateAccountAsync(acc);
                //_logger.LogAccountDeactivation(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);


                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
               // _logger.LogAccountDeactivation(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, "No Target");

                return false;
            }
        }
        /*
		 * Enables the targeted account. 
		 * currentUser is used to validate that the person calling this method has permission to do so.
		 * targetPK is the email of the user whos account is being activated
		 */
        public async Task<bool> EnableAccountAsync(Account currentUser, int userID)
        {
            if (! await _permissionService.HasPermissionAsync(currentUser.UserID, "enableAccount") || !currentUser.IsActive)
            {
                //_logger.LogAccountEnabling(UM_CATEGORY, "Console", false, "ADMIN ACCESS NEEDED", currentUser.Email, "No Target");
                return false;
            }
            try
            {
                if (! await _accountAccess.AccountExistsAsync(userID))
                {
                    return false;
                }
                Account? acc = await _accountAccess.GetAccountAsync(userID);

                acc.Enabled = true;
                await _accountAccess.UpdateAccountAsync(acc);
                //_logger.LogAccountEnabling(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);


                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogAccountEnabling(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, "No Target");
                return false;
            }
        }

        public async Task<bool> AccountExistsAsync(int userID)
        {
            return await _accountAccess.AccountExistsAsync(userID);
        }


        /*promotes the target user to admin
		 * takes in currentUser to verify the current session is being handled by an admin
		 * targetPK is the email(primary key) of the user being targeted
		 */

        public async Task<bool> PromoteToAdmin(Account currentUser, int userID)
        {
            try
            {
                if (await _permissionService.HasPermissionAsync(currentUser.UserID, "createAdmin") && currentUser.IsActive)
                {
                    Account? acc = await _accountAccess.GetAccountAsync(userID);
                    if (acc == null)
                    {
                        Console.WriteLine("No such account exists");
                        return false;
                    }

                    await _permissionService.AssignDefaultAdminPermissions(userID);
                    await _accountAccess.UpdateAccountAsync(acc);
                    //_logger.LogAccountPromote(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);

                    Console.WriteLine("UM operation was successful");
                    return true;
                }
                //_logger.LogAccountPromote(UM_CATEGORY, "Console", false, "User is not admin and/or target account is not active", currentUser.Email, targetPK);
                return false;
            }
            catch (Exception ex)
            {
               // _logger.LogAccountPromote(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, targetPK);
                return false;
            }
        }

        public async Task<bool> AddFlagToAccountAsync(int userID, int IngredientID)
        {
            FoodFlag foodFlag = new(userID, IngredientID);
            return await _flagService.AddFlagAsync(foodFlag);
        }

        public async Task<bool> RemoveFoodFlag(int userID, int IngredientID)
        {
            return await _flagService.RemoveFoodFlagAsync(userID, IngredientID);
        }

        public async Task<bool> accountHasFlag(int userID, int IngredientID)
        {
            return await _flagService.AccountHasFlagAsync(userID, IngredientID);
        }

        public List<FoodFlag> GetAllAccountFlags(int userID)
        {
            return _flagService.GetAllAccountFlags(userID);
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
}
