using Console_Runner.DAL;
using Console_Runner.Logging;
using Console_Runner.User_Management;
using Xunit;
//---------------------------------NOTICE, THIS FILE NOT BEEN UPDATED WITH THE CHANGES TO AUTHORIZATION AND WILL NOT WORK AS EXPECTED-----------------------------------
namespace UnitTest
{
    public class UMShould
    {


        [Fact]
        public void CreateUserSuccess()
        {

            //Arange
            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);

            string tester = "unitTester";
            Account acc = new Account();
            acc.Email = tester;
            acc.Password = "password1";
            acc.Fname = "fname";
            acc.Lname = "lname";
            //Act
            um.UserSignUp(acc);
            //Assert
            Assert.True(accountGateway.AccountExists(acc.Email));
                
        }

        [Fact]
        // trys to delete a user, while acting as if an admin executed the command
        public void DeleteUserSuccess()
        {

            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);

            Account admin = new Account();
            admin.Email = "deleteUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            admin.IsActive = true;

            permService.AssignDefaultAdminPermissions(admin.Email);
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
            Assert.True(!accountGateway.AccountExists(acc.Email));
        }
        [Fact]
        public void UpdateSuccess()
        {
            //Arrange
            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);


            Account admin = new Account();
            admin.Email = "updateUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";

            permService.AssignDefaultAdminPermissions(admin.Email);
            um.UserSignUp(admin);
            admin.IsActive = true;


            Account acc = new Account();
            acc.Email = "updateUserSuccessEmail";
            acc.Password = "t";
            um.UserSignUp(acc);

            string nName = "new name";
            string nlName = "new last name";
            string nPassword = "NewPassword";

            //act
            um.UserUpdateData(admin, acc.Email, nName, nlName, nPassword);
            acc = accountGateway.GetAccount(acc.Email);
            //Assert
            Assert.True(acc.Fname == nName && acc.Lname == nlName && acc.Password == nPassword);
            Assert.True(acc.IsActive == false);
        }
        [Fact]
        public void DisableSuccess()
        {
            //arange 
            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);

            Account admin = new Account();
            admin.Email = "disableSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            um.UserSignUp(admin);
            permService.AssignDefaultAdminPermissions(admin.Email);
            
            admin.IsActive = true;

            Account acc = new Account();
            acc.Email = "DisableSuccessUserEmail";
            acc.Password = "t";
            acc.Enabled = true;
            um.UserSignUp(acc);

            //act
            um.DisableAccount(admin, acc.Email);
            //Assert
            Assert.True(!acc.Enabled);
            Assert.True(acc.IsActive == false);
        }

        [Fact]
        public void EnableSuccess()
        {
            //Arange 
            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);

            Account admin = new Account();
            admin.Email = "enableSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";

            permService.AssignDefaultAdminPermissions(admin.Email);
            um.UserSignUp(admin);
            admin.IsActive = true;

            Account acc = new Account();
            acc.Email = "enableeSuccessUserEmail";
            acc.Password = "t";
            acc.Enabled = false;
            um.UserSignUp(acc);

            //act
            um.EnableAccount(admin, acc.Email);
            //Assert
            Assert.True(acc.Enabled);
        }
        [Fact]
        public void GetUserSuccess()
        {
            //arange 
            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);

            Account acc = new Account();
            acc.Email = "getUserSuccess";
            acc.Password = "t";
            acc.Enabled = false;
            um.UserSignUp(acc);

            //act
            Account temp = um.GetUserAccount(acc.Email);
            //Assert
            Assert.True(temp == acc);
        }
        [Fact]
        public void AuthenticatePasswordSuccess()
        {
            //Arange 
            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);

            Account acc = new Account();
            acc.Email = "authenticatePasswordSuccess";
            acc.Password = "password!";
            um.UserSignUp(acc);

            //Assert
            Assert.True(um.AuthenticateUserPass(acc.Email, acc.Password));
            Assert.False(um.AuthenticateUserPass(acc.Email, "t"));
        }
        [Fact]
        public void SignInSuccess()
        {
            //arange 
            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);

            Account acc = new Account();
            acc.Email = "signInSuccess";
            acc.Password = "pass";
            um.UserSignUp(acc);
            
            //act
            um.SignIn(acc.Email,acc.Password);
            //Assert
            Assert.True(acc.IsActive);
        }
        [Fact]
        public void PromoteToAdminSuccess()
        {
            //arange 
            IAccountGateway accountGateway = new MemAccountGateway();
            IPermissionGateway efPermissionGateway = new MemPermissionGateway();
            PermissionService permService = new PermissionService(efPermissionGateway);
            IlogGateway logAccess = new EFLogGateway();
            Logging logger = new Logging(logAccess);
            UM um = new(accountGateway, permService, logger);

            Account admin = new Account();
            admin.Email = "PromoteToAdminSuccessAdmin";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";

            permService.AssignDefaultAdminPermissions(admin.Email) ;
            um.UserSignUp(admin);
            admin.IsActive = true;

            Account acc = new Account();
            acc.Email = "PromoteToAdminSuccess";
            acc.Password = "pass";
            um.UserSignUp(acc);

            //act
            um.PromoteToAdmin(admin, acc.Email);

            //Assert
            Assert.True(efPermissionGateway.HasPermission(acc.Email, "createAdmin"));
        }

    }
}
