using Class1;
using Console_Runner;
using User;
using Xunit;
using Console_Runner.DAL;
using LogAndArchive;
//---------------------------------NOTICE, THIS FILE NOT BEEN UPDATED WITH THE CHANGES TO AUTHORIZATION AND WILL NOT WORK AS EXPECTED-----------------------------------
namespace UnitTest
{
    public class UMShould
    {


        [Fact]
        public void CreateUserSuccess()
        {

            //Arange
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            string tester = "unitTester";
            Account acc = new Account();
            acc.Email = tester;
            acc.Password = "password1";
            acc.Fname = "fname";
            acc.Lname = "lname";
            //Act
            um.UserSignUp(acc);
            //Assert
            Assert.True(dal.accountExists(acc.Email));
                
        }

        [Fact]
        // trys to delete a user, while acting as if an admin executed the command
        public void deleteUserSuccess()
        {
            //Arange
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account admin = new Account();
            admin.Email = "deleteUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            admin.isActive = true;
            user_permissions permissions = new(dal);
            permissions.defaultAdminPermissions(admin.Email);
            um.UserSignUp(admin);

            Account acc = new Account();
            acc.Email = "deleteUserSuccessEmail";
            acc.Password = "t";
            acc.Fname = "fname";
            acc.Lname = "lname";
            um.UserSignUp(acc);
            //Act
            um.UserDelete(admin, acc.Email);
            //Assert
            Assert.True(dal.accountExists(acc.Email) == false);
        }
        [Fact]
        public void updateSuccess()
        {
            //Arange
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account admin = new Account();
            admin.Email = "updateUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            user_permissions permissions = new(dal);
            permissions.defaultAdminPermissions(admin.Email);
            um.UserSignUp(admin);
            admin.isActive = true;


            Account acc = new Account();
            acc.Email = "updateUserSuccessEmail";
            acc.Password = "t";
            um.UserSignUp(acc);

            string nName = "new name";
            string nlName = "new last name";
            string nPassword = "NewPassword";

            //act
            um.UserUpdateData(admin, acc.Email, nName, nlName, nPassword);
            acc = dal.getAccount(acc.Email);
            //Assert
            Assert.True(acc.Fname == nName && acc.Lname == nlName && acc.Password == nPassword);
            Assert.True(acc.isActive == false);
        }
        [Fact]
        public void disableSuccess()
        {
            //arange 
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account admin = new Account();
            admin.Email = "disableSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            user_permissions permissions = new(dal);
            permissions.defaultAdminPermissions(admin.Email);
            um.UserSignUp(admin);
            admin.isActive = true;

            Account acc = new Account();
            acc.Email = "DisableSuccessUserEmail";
            acc.Password = "t";
            acc.enabled = true;
            um.UserSignUp(acc);

            //act
            um.DisableAccount(admin, acc.Email);
            //Assert
            Assert.True(dal.getAccount(acc.Email).enabled == false);
            Assert.True(acc.isActive == false);
        }
        [Fact]
        public void enableSuccess()
        {
            //arange 
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account admin = new Account();
            admin.Email = "enableSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            user_permissions permissions = new(dal);
            permissions.defaultAdminPermissions(admin.Email);
            um.UserSignUp(admin);
            admin.isActive = true;

            Account acc = new Account();
            acc.Email = "enableeSuccessUserEmail";
            acc.Password = "t";
            acc.enabled = false;
            um.UserSignUp(acc);

            //act
            um.EnableAccount(admin, acc.Email);
            //Assert
            Assert.True(dal.getAccount(acc.Email).enabled);
        }
        [Fact]
        public void getUserSuccess()
        {
            //arange 
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account acc = new Account();
            acc.Email = "getUserSuccess";
            acc.Password = "t";
            acc.enabled = false;
            um.UserSignUp(acc);

            //act
            Account temp = um.getUserAcc(acc.Email);
            //Assert
            Assert.True(temp == acc);
        }
        [Fact]
        public void authenticatePasswordSuccess()
        {
            //arange 
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account acc = new Account();
            acc.Email = "authenticatePasswordSuccess";
            acc.Password = "password!";
            um.UserSignUp(acc);

            //Assert
            Assert.True(um.AuthenticateUserPass(acc.Email, acc.Password));
            Assert.False(um.AuthenticateUserPass(acc.Email, "t"));
        }
        [Fact]
        public void signInSuccess()
        {
            //arange 
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account acc = new Account();
            acc.Email = "signInSuccess";
            acc.Password = "pass";
            um.UserSignUp(acc);
            
            //act
            um.signIn(acc.Email,acc.Password);
            //Assert
            Assert.True(acc.isActive);
        }
        [Fact]
        public void promoteToAdminSuccess()
        {
            //arange 
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account admin = new Account();
            admin.Email = "PromoteToAdminSuccessAdmin";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            user_permissions permissions = new(dal);
            permissions.defaultAdminPermissions(admin.Email);
            um.UserSignUp(admin);
            admin.isActive = true;

            Account acc = new Account();
            acc.Email = "PromoteToAdminSuccess";
            acc.Password = "pass";
            um.UserSignUp(acc);

            //act
            um.promoteToAdmin(admin, acc.Email);

            //Assert
            Assert.True(dal.hasPermission(acc.Email, "createAdmin"));
        }

    }
}
