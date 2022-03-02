//using Class1;
using Console_Runner.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;
using LogAndArchive;


namespace Console_Runner
{

    //Current user management class
    public class UM
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountRepo _accountAccess;
        private readonly IPermissionRepo _permissionAccess;
        private readonly ILogger _logger;
        public UM(IAccountRepo accountAccess, IPermissionRepo permissionAccess, ILogger logging)
        {
            Console.WriteLine("Creating UM object");
            _accountAccess = accountAccess;
            _permissionAccess = permissionAccess;
            _logger = logging;
        }


        //User sign up will take in an account object and persist it to the db.
        public bool UserSignUp(Account acc)
        {
            try
            {
                if (_accountAccess.AccountExists(acc.Email))
                {
                    Console.WriteLine("email already in use");
                    return false;
                }
                user_permissions newRule = new user_permissions(_permissionAccess);
                newRule.AssignDefaultUserPermissions(acc.Email);
                acc.isActive = false;
                _accountAccess.AddAccount(acc);
                _logger.logAccountCreation(UM_CATEGORY, "test page", true, "", acc.Email);
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.logAccountCreation(UM_CATEGORY, "test page", false, ex.Message, acc.Email);
                return false;
            }

        }

        /*
		 * Delets a user corosponding to the email provided as arg
		 * Takes in currentUser to validate user calling method has permission to do so
		 */
        public bool UserDelete(Account currentUser, string targetEmail)
        {

            if (!_permissionAccess.HasPermission(currentUser.Email, "deleteAccount"))
            {
                _logger.logAccountDeletion(UM_CATEGORY, "test page", false, "ADMIN ACCESS NEEDED", currentUser.Email);
                return false;
            }
            try
            {
                if (!_accountAccess.AccountExists(targetEmail))
                {
                    return false;
                }

                Account acc = _accountAccess.GetAccount(targetEmail);
                
                if (_permissionAccess.HasPermission(targetEmail,"createAdmin") && (_permissionAccess.AdminCount() < 2))
                {
                    Console.WriteLine("Deleting this account would result in there being no admins.");
                    return false;
                }

                _permissionAccess.RemoveAllUserPermissions(acc.Email);
                _accountAccess.RemoveAccount(acc);
                _logger.logAccountDeletion(UM_CATEGORY, "test page", true, "", acc.Email);
                
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.logAccountDeletion(UM_CATEGORY, "test page", false, ex.Message, targetEmail);
                return false;
            }
        }

        //will return an account object from the DB given a PK from the argument field
        public Account GetUserAccount(string targetPK)
        {
            try
            {

                if(_accountAccess.AccountExists(targetPK))
                {
                    return _accountAccess.GetAccount(targetPK);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.logGeneric(UM_CATEGORY, "test page", false, ex.Message, targetPK, "Failed to read.");
                return null;
            }
        }

        //will update a user's data from a given PK in the argument, fields being changed are given in the argument line as well, null input means no change
        public bool UserUpdateData(Account currentUser, string targetPK, string nFname, string nLname, string npassword)
        {
            bool fNameChanged = false, lNameChanged = false, passwordChanged = false;
            string fTemp = "", lTemp = "", pTemp = "";
            user_permissions permissions = new user_permissions(_permissionAccess);
            if (currentUser.Email != targetPK)
            {
                if (!_permissionAccess.HasPermission(currentUser.Email,"editOtherAccount") || !currentUser.isActive)
                {
                    _logger.logGeneric(UM_CATEGORY, "test page", false, "ADMIN ACCESS NEEDED", currentUser.Email, "ADMIN ACCESS NEEDED TO UPDATE USER DATA");
                    return false;
                }
            }
            try
            {

                Account acc = _accountAccess.GetAccount(targetPK);
                if (acc == null)
                {
                    Console.WriteLine("NULL ACCOUNT FOUND");
                    return false;
                }
                if (nFname != "")
                {
                    fTemp = acc.Fname;
                    acc.Fname = nFname;
                    fNameChanged = true;
                }
                if (nLname != "")
                {
                    lTemp = acc.Lname;
                    acc.Lname = nLname;
                    lNameChanged = true;
                }
                if (npassword != "")
                {
                    pTemp = acc.Password;
                    acc.Password = npassword;
                    passwordChanged = true;
                }

                _accountAccess.UpdateAccount(acc);

                if (fNameChanged)
                    _logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, fTemp, nFname);
                if (lNameChanged)
                    _logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, lTemp, nLname);
                if (passwordChanged)
                    _logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, pTemp, npassword);

                acc.isActive = false;
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                _logger.logGeneric(UM_CATEGORY, "test page", false, ex.Message, targetPK, "Could not change user info");
                return false;
            }
        }


        //authenticates a users input password for login. True if pass matches, false otherwise
        public bool AuthenticateUserPass(string user, string userPass)
        {
            Account acc = GetUserAccount(user);
            return (acc != null && acc.Password == userPass);
        }
        //takes in username and password. If valid returns an account object for the user with specified data.
        public Account SignIn(string user, string userPass)
        {
            if (AuthenticateUserPass(user, userPass))
            {
                _logger.logLogin(UM_CATEGORY, "test page", true, "", user);
                Account acc = GetUserAccount(user);
                acc.isActive = true;
                return acc;
            }
            else
            {
                _logger.logLogin(UM_CATEGORY, "test page", false, "Invalid Password", user);
                return null;
            }
        }

        /*
		 * Disables the account with email of targetPK if exists
		 * currentUser is taken in to validate the user calling this has permission to do so
		 */
        public bool DisableAccount(Account currentUser, string targetPK)
        {
            if (!_permissionAccess.HasPermission(currentUser.Email,"disableAccount") || !currentUser.isActive)
            {
                _logger.logAccountDeactivation(UM_CATEGORY, "Console", false, "ADMIN ACCESS NEEDED", currentUser.Email, "No Target");
                return false;
            }
            try
            {
                if (!_accountAccess.AccountExists(targetPK))
                {
                    return false;
                }
                Account acc = _accountAccess.GetAccount(targetPK);
 
                if (_permissionAccess.IsAdmin(targetPK) && (_permissionAccess.AdminCount() < 2))
                {
                    Console.WriteLine("Disabling this account would result in there being no admins.");
                    return false;
                }
                acc.enabled = false;
                acc.isActive = false;
                _accountAccess.UpdateAccount(acc);
                _logger.logAccountDeactivation(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);

                
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                _logger.logAccountDeactivation(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, "No Target");
                return false;
            }
        }
        /*
		 * Enables the targeted account. 
		 * currentUser is used to validate that the person calling this method has permission to do so.
		 * targetPK is the email of the user whos account is being activated
		 */
        public bool EnableAccount(Account currentUser, string targetPK)
        {
            user_permissions permissions = new user_permissions(_permissionAccess);
            if (!_permissionAccess.HasPermission(currentUser.Email, "enableAccount") || !currentUser.isActive)
            {
                _logger.logAccountEnabling(UM_CATEGORY, "Console", false, "ADMIN ACCESS NEEDED", currentUser.Email, "No Target");
                return false;
            }
            try
            {
                if (!_accountAccess.AccountExists(targetPK))
                {

                    return false;
                }
                Account acc = _accountAccess.GetAccount(targetPK);

                acc.enabled=true;
                _accountAccess.UpdateAccount(acc);
                _logger.logAccountEnabling(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);


                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                _logger.logAccountEnabling(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, "No Target");
                return false;
            }
        }


        /*promotes the target user to admin
		 * takes in currentUser to verify the current session is being handled by an admin
		 * targetPK is the email(primary key) of the user being targeted
		 */

        public bool PromoteToAdmin(Account currentUser, string targetPK)
        {
            try
            {
                if (_permissionAccess.HasPermission(currentUser.Email,"createAdmin") && currentUser.isActive)
                {
                    if (!_accountAccess.AccountExists(targetPK))
                    {
                        Console.WriteLine("No such account exists");
                        return false;
                    }
                    Account acc = _accountAccess.GetAccount(targetPK);
                        
                    user_permissions permissions = new(_permissionAccess);
                    permissions.AssignDefaultAdminPermissions(targetPK);
                    _accountAccess.UpdateAccount(acc);
                    _logger.logAccountPromote(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);
                    
                Console.WriteLine("UM operation was successful");
                return true;
                }
                _logger.logAccountPromote(UM_CATEGORY, "Console", false, "User is not admin and/or target account is not active", currentUser.Email, targetPK);
                return false;
            }
            catch (Exception ex)
            {
                _logger.logAccountPromote(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, targetPK);
                return false;
            }
        }
    }
}