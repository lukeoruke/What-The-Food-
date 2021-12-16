using Class1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner
{
    //Current user management class
    public class UM
    {
        const string UM_CATEGORY = "Data Store";
        Logging logger;

        public UM()
        {
            Console.WriteLine("Creating UM object");
            logger = new Logging();
        }

        //User sign up will take in appropriate information from console and add new user to DB
        public bool UserSignUp()
        {
            Account acc = new Account();
            try
            {

                using (var context = new Context())
                {
                    Console.WriteLine("Enter Email");
                    acc.Email = Console.ReadLine();
                    while(context.accounts.Find(acc.Email) != null)
                    {
                        Console.WriteLine("EMAIL ALREADY IN USE Enter a different email");
                        acc.Email = Console.ReadLine();
                    }
                    if (acc == null)
                    {
                        Console.WriteLine("No such account exists");
                        return false;
                    }
                    Console.WriteLine("Enter password");
                    acc.Password = Console.ReadLine();
                    Console.WriteLine("Enter fname");
                    acc.Fname = Console.ReadLine();
                    Console.WriteLine("Enter lname");
                    acc.Lname = Console.ReadLine();
                    acc.isActive = true;
                    context.accounts.Add(acc);
                    context.SaveChanges();
                    logger.logAccountCreation(UM_CATEGORY, "test page", true, "", acc.Email);
                }
                Console.WriteLine("UM operation was successful");
                return true;
            }catch(Exception ex)
            {
                logger.logAccountCreation(UM_CATEGORY, "test page", false, ex.Message, acc.Email);
                return false;
            }
            
        }

        //User sign up will take in an account object and persist it to the db.
        public bool UserSignUp(Account acc)
        {
            try
            {
                using(var context = new Context())
                {
                    if (context.accounts.Find(acc.Email) != null)
                    {
                        Console.WriteLine("email already in use");
                        return false;
                    }
                    acc.isActive = true;
                    context.accounts.Add(acc);
                    context.SaveChanges();
                    logger.logAccountCreation(UM_CATEGORY, "test page", true, "", acc.Email);
                }
                Console.WriteLine("UM operation was successful");
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.logAccountCreation(UM_CATEGORY, "test page", false, ex.Message, acc.Email);
                return false;
            }
            
        }
        /*
         * takes params to create a new account object. If successful persists account to db
         */
        public bool UserSignUp(string email, string first, string last, string pass)
        {
            try
            {

                using (var context = new Context())
                {
                    if (context.accounts.Find(email) != null)
                    {
                        Console.WriteLine("email already in use");
                        return false;
                    }
                    var acc = new Account()
                    {
                        Email = email,
                        Fname = first,
                        Lname = last,
                        Password = pass,
                        isActive = true,
                        accessLevel = 1
                    };
                    context.accounts.Add(acc);
                    context.SaveChanges();
                    logger.logAccountCreation(UM_CATEGORY, "test page", true, "", acc.Email);
                }
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                logger.logAccountCreation(UM_CATEGORY, "test page", false, ex.Message, "");
                return false;
            }

        }

        /*
         * Delets a user corosponding to the email provided as arg
         * Takes in currentUser to validate user calling method has permission to do so
         */
        public bool UserDelete(Account currentUser, string targetPK)
        {
            string email = targetPK;
            if (!currentUser.isAdmin() || !currentUser.isActive)
            {
                logger.logAccountDeletion(UM_CATEGORY, "test page", false, "ADMIN ACCESS NEEDED", currentUser.Email);
                return false;
            }
            try
            {
                using (var context = new Context())
                {
                    Account acc = context.accounts.Find(targetPK);
                    if (acc == null)
                    {
                        return false;
                    }
                    if (acc.isAdmin() && (AdminCount() < 2))
                    {
                        Console.WriteLine("Deleting this account would result in there being no admins.");
                        return false;
                    }
                    context.Remove(acc);
                    context.SaveChanges();
                    logger.logAccountDeletion(UM_CATEGORY, "test page", true, "", email);
                }
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                logger.logAccountDeletion(UM_CATEGORY, "test page", false, ex.Message, email);
                return false;
            }
        }
        /*will delete a user through console for demo purposes
         * takes currentUser to validate user calling this function has permission to do so
         */
        public bool UserDelete(Account currentUser)
        {
            if (!currentUser.isAdmin() || !currentUser.isActive)
            {
                logger.logAccountDeletion(UM_CATEGORY, "test page", false, "ADMIN ACCESS NEEDED", currentUser.Email);
                return false;
            }
            try
            {
                using (var context = new Context())
                {
                    Console.WriteLine("Enter email address of the account desired to delete");
                    string targetPK = Console.ReadLine();
                    Account acc = context.accounts.Find(targetPK);
                    if (acc == null)
                    {
                        return false;
                    }
                    if (acc.isAdmin() && (AdminCount() < 2))
                    {
                        Console.WriteLine("Deleting this account would result in there being no admins.");
                        return false;
                    }
                    Console.WriteLine("Deletion successful.");
                    context.Remove(acc);
                    context.SaveChanges();
                    logger.logAccountDeletion(UM_CATEGORY, "test page", true, "", currentUser.Email);
                    Console.WriteLine("UM operation was successful");
                    return true ;
                }
            }
            catch (Exception ex)
            {
                logger.logAccountDeletion(UM_CATEGORY, "test page", false, ex.Message, currentUser.Email);
                return false;
            }
        }

        //will return an account object from the DB given a PK from the argument field
        public Account UserReadData(string targetPK)
        {
            try
            {
                using (var context = new Context())
                {
                    Account acc = context.accounts.Find(targetPK);
                    Console.WriteLine("UM operation was successful");
                    return acc;
                }
            }
            catch (Exception ex)
            {
                logger.logGeneric(UM_CATEGORY, "test page", false, ex.Message, targetPK, "Failed to read.");
                return null;
            }
        }
        /*
         * uses console to read and display information on a user to console. For demo purposes
         */
        public Account UserReadData()
        {
            try
            {
                using (var context = new Context())
                {
                    Console.WriteLine("Enter email of user you wish to look up");
                    string targetPK = Console.ReadLine();
                    Account acc = context.accounts.Find(targetPK);

                    Console.WriteLine(acc.ToString());
                    Console.WriteLine("UM operation was successful");
                    return acc;
                }
            }
            catch (Exception ex)
            {
                logger.logGeneric(UM_CATEGORY, "test page", false, ex.Message, "", "Failed to read.");
                return null;
            }
        }



        //will update a user's data from a given PK in the argument, fields being changed are given in the argument line as well, null input means no change
        public bool UserUpdateData(Account currentUser, string targetPK, string nFname, string nLname, string npassword)
        {
            bool fNameChanged = false, lNameChanged = false, passwordChanged = false;
            string fTemp = "", lTemp = "", pTemp = "";
            if (currentUser.Email != targetPK)
            {
                if (!currentUser.isAdmin() || !currentUser.isActive)
                {
                    logger.logGeneric(UM_CATEGORY, "test page", false, "ADMIN ACCESS NEEDED", currentUser.Email, "ADMIN ACCESS NEEDED TO UPDATE USER DATA");
                    return false;
                }
            }
            try
            {
                using (var context = new Context())
                {
                    Account acc = context.accounts.Find(targetPK);
                    if (acc == null)
                    {
                        Console.WriteLine("NULL ACCOUNT FOUND");
                         return false;
                    }
                    if (nFname != null)
                    {
                        fTemp = acc.Fname;
                        acc.Fname = nFname;
                        fNameChanged = true;
                    }
                    if (nLname != null)
                    {
                        lTemp = acc.Lname;
                        acc.Lname = nLname;
                        lNameChanged = true;
                    }if(npassword != null)
                    {
                        pTemp = acc.Password;
                        acc.Password = npassword;
                        passwordChanged = true;
                    }
                    
                    context.accounts.Update(acc);
                    context.SaveChanges();

                    if(fNameChanged)
                        logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, fTemp, nFname);
                    if(lNameChanged)
                        logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, lTemp, nLname);
                    if (passwordChanged)
                        logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, pTemp, npassword);

                }
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                logger.logGeneric(UM_CATEGORY, "test page", false, ex.Message, targetPK, "Could not change user info");
                return false;
            }
        }
        /*
         * Updates the target users data through command line. This is for demo purposes.
         * currentUser is taken in to validate the user calling this method has access to do so.
         */
        public bool UserUpdateData(Account currentUser)
        {
            bool fNameChanged = false, lNameChanged = false, passwordChanged = false; 
            string fTemp = "", lTemp = "", pTemp = "";
            if (!currentUser.isAdmin() || !currentUser.isActive)
            {
                logger.logGeneric(UM_CATEGORY, "test page", false, "ADMIN ACCESS NEEDED", currentUser.Email, "ADMIN ACCESS NEEDED TO UPDATE USER DATA");
                return false;
            }
            try
            {
                using (var context = new Context())
                {
                    Console.WriteLine("Enter email address");
                    string targetPK = Console.ReadLine();
                    Account acc = context.accounts.Find(targetPK);
                    if (acc == null)
                    {
                        Console.WriteLine("No such account exists");
                        return false;
                    }
                    Console.WriteLine("Enter new First name or enter to skip");
                    string nFname = Console.ReadLine();
                    Console.WriteLine("Enter new Last Name or enter to skip");
                    string nLname = Console.ReadLine();
                    Console.WriteLine("Enter new password or enter to skip");
                    string npassword = Console.ReadLine();

                    if (acc == null)
                        Console.WriteLine("NULL ACCOUNT FOUND");
                    if (nFname != null)
                    {
                        fTemp = acc.Fname;
                        acc.Fname = nFname;
                        fNameChanged = true;
                    }
                    if (nLname != null)
                    {
                        lTemp = acc.Lname;
                        acc.Lname = nLname;
                        lNameChanged=true;
                    }
                    if (nLname != null)
                    {
                        pTemp = acc.Password; 
                        acc.Password = npassword;
                        passwordChanged = true;
                    }
                    context.accounts.Update(acc);
                    context.SaveChanges();
                    if (fNameChanged)
                        logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, fTemp, nFname);
                    if (lNameChanged)
                        logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, lTemp, nLname);
                    if (passwordChanged)
                        logger.logAccountNameChange(UM_CATEGORY, "test page", true, "", acc.Email, pTemp, npassword);
                }
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                logger.logGeneric(UM_CATEGORY, "test page", false, ex.Message, currentUser.Email, "Could not change user info");
                return false;
            }
        }

        //authenticates a users input password for login. True if pass matches, false otherwise
        public bool AuthenticateUserPass(string user,string userPass)
        {
            Account acc = UserReadData(user);
            return (acc != null && acc.Password == userPass);
        }
        //takes in username and password. If valid returns an account object for the user with specified data.
        public Account signIn(string user, string userPass)
        {
            if (AuthenticateUserPass(user, userPass))
            {
                logger.logLogin(UM_CATEGORY, "test page", true, "", user);
                return UserReadData(user);
            }
            else
            {
                logger.logLogin(UM_CATEGORY, "test page", false, "Invalid Password", user);
                return null;
            }
        }
        //retrieves and prints a list of all users in the databse to the console.
        public bool GetAllUsers()
        {
            try
            {
                using (var context = new Context())
                {
                    foreach (var account in context.accounts)
                    {
                        Console.WriteLine(account.ToString());
                    }
                }
                Console.WriteLine("UM operation was successful");
                return true;
            }catch(Exception ex)
            {
                logger.logGeneric(UM_CATEGORY, "test page", false, ex.Message, "No user", "Could not retrieve all users");
                return false;
            }
        }
        /*
         * Disables the account with email of targetPK if exists. This method is for console demoing.
         * currentUser is taken in to validate the user calling this has permission to do so
         * targetPK is gotten through command line
         */
        public bool DisableAccount(Account currentUser)
        {
            if (!currentUser.isAdmin() || !currentUser.isActive)
            {
                logger.logAccountDeactivation(UM_CATEGORY, "Console", false, "ADMIN ACCESS NEEDED", currentUser.Email, "No Target");
                return false;
            }
            try
            {
                Console.WriteLine("Enter email address");
                string targetPK = Console.ReadLine();
                using (var context = new Context())
                {
                    Account acc = context.accounts.Find(targetPK);
                    if (acc == null)
                    {
                        Console.WriteLine("No such account exists");
                        return false;
                    }
                    if (acc.isAdmin() && (AdminCount() < 2))
                    {
                        Console.WriteLine("Disabling this account would result in there being no admins.");
                        return false;
                    }
                    acc.isActive = false;
                    context.accounts.Update(acc);
                    context.SaveChanges();
                    logger.logAccountDeactivation(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);

                }
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                logger.logAccountDeactivation(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, "No Target");
                return false;
            }
        }
        /*
         * Disables the account with email of targetPK if exists
         * currentUser is taken in to validate the user calling this has permission to do so
         */
        public bool DisableAccount(Account currentUser, string targetPK)
        {
            if (!currentUser.isAdmin() || !currentUser.isActive)
            {
                logger.logAccountDeactivation(UM_CATEGORY, "Console", false, "ADMIN ACCESS NEEDED", currentUser.Email, "No Target");
                return false;
            }
            try
            {
                using (var context = new Context())
                {
                    Account acc = context.accounts.Find(targetPK);
                    if (acc == null)
                    {
                        Console.WriteLine("No such account exists");
                        return false;
                    }
                    if (acc.isAdmin() && (AdminCount() < 2))
                    {
                        Console.WriteLine("Disabling this account would result in there being no admins.");
                        return false;
                    }
                    acc.isActive = false;
                    context.accounts.Update(acc);
                    context.SaveChanges();
                    logger.logAccountDeactivation(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);

                }
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                logger.logAccountDeactivation(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, "No Target");
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
            if (!currentUser.isAdmin() || !currentUser.isActive)
            {
                logger.logAccountEnabling(UM_CATEGORY, "Console", false, "ADMIN ACCESS NEEDED", currentUser.Email, "No Target");
                return false;
            }
            try
            {
                using (var context = new Context())
                {
                    Account acc = context.accounts.Find(targetPK);
                    if (acc == null)
                    {
                        Console.WriteLine("No such account exists");
                        return false;
                    }
                    acc.isActive = true;
                    context.accounts.Update(acc);
                    context.SaveChanges(true);
                    logger.logAccountEnabling(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);

                }
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                logger.logAccountEnabling(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, "No Target");
                return false;
            }
        }
        /*
         * Enables the targeted account. Gets targetPK through command line. This version is for console demo
         * currentUser is used to validate that the person calling this method has permission to do so.
         */
        public bool EnableAccount(Account currentUser)
        {
            if (!currentUser.isAdmin() || !currentUser.isActive)
            {
                logger.logAccountEnabling(UM_CATEGORY, "Console", false, "ADMIN ACCESS NEEDED", currentUser.Email, "No Target");
                return false;
            }
            try
            {
                Console.WriteLine("Enter email address of account to reenable");
                string targetPK = Console.ReadLine();
                using (var context = new Context())
                {
                    Account acc = context.accounts.Find(targetPK);
                    if (acc == null)
                    {
                        Console.WriteLine("No such account exists");
                        return false;
                    }
                    acc.isActive = true;
                    context.accounts.Update(acc);
                    context.SaveChanges(true);
                    logger.logAccountEnabling(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);

                }
                Console.WriteLine("UM operation was successful");
                return true;
            }
            catch (Exception ex)
            {
                logger.logAccountEnabling(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, "No Target");
                return false;
            }
        }
         /*promotes the target user to admin. This version is for use with command line for demo purposes.
         * takes in currentUser to verify the current session is being handled by an admin
         * targeted email comes through command line
         */
        public bool promoteToAdmin(Account currentUser)
        {
            Console.WriteLine("Enter target email");
            String targetPK = Console.ReadLine();
            try
            {
                if (currentUser.isAdmin() && currentUser.isActive)
                {
                    using (var context = new Context())
                    {
                        Account acc = context.accounts.Find(targetPK);
                        if (acc == null)
                        {
                            Console.WriteLine("No such account exists");
                            return false;
                        }
                        acc.accessLevel = 2;
                        context.Update(acc);
                        context.SaveChanges();
                        logger.logAccountPromote(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);
                    }
                    Console.WriteLine("UM operation was successful");
                    return true;
                }
                logger.logAccountPromote(UM_CATEGORY, "Console", false, "User is not admin and/or target account is not active", currentUser.Email, targetPK);
                return false;
                
            }catch (Exception ex)
            {
                logger.logAccountPromote(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, targetPK);
                return false;
            }
        }
        /*promotes the target user to admin
         * takes in currentUser to verify the current session is being handled by an admin
         * targetPK is the email(primary key) of the user being targeted
         */
        
        public bool promoteToAdmin(Account currentUser, string targetPK)
        {
            try
            {
                if (currentUser.isAdmin() && currentUser.isActive)
                {
                    using (var context = new Context())
                    {
                        Account acc = context.accounts.Find(targetPK);
                        if (acc == null)
                        {
                            Console.WriteLine("No such account exists");
                            return false;
                        }
                        acc.accessLevel = 2;
                        context.Update(acc);
                        context.SaveChanges();
                        logger.logAccountPromote(UM_CATEGORY, "Console", true, "", currentUser.Email, targetPK);
                    }
                    Console.WriteLine("UM operation was successful");
                    return true;
                }
                logger.logAccountPromote(UM_CATEGORY, "Console", false, "User is not admin and/or target account is not active", currentUser.Email, targetPK);
                return false;

            }
            catch (Exception ex)
            {
                logger.logAccountPromote(UM_CATEGORY, "Console", false, ex.Message, currentUser.Email, targetPK);
                return false;
            }
        }
        //returns the number of admins in the database
        public int AdminCount()
        {
            int count = 0;
            using (var context = new Context())
            {
                foreach (var account in context.accounts)
                {
                    if (account.accessLevel >= 2 && account.isActive) count++;
                }
            }
            return count;
        }
    }   
}