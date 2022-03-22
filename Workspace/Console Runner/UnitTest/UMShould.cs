using Xunit;
using Console_Runner.AccountService;
namespace Test.UM
{
    public class UMUnitTests
    {

        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountGateway _accountAccess = new MemAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new MemAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new MemFlagGateway();


       [Fact]
        public async void CreateUserSuccess()
        {

            //Arange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            string tester = "unitTester";
            Account acc = new Account();
            acc.Email = tester;
            acc.Password = "password1";
            acc.FName = "fname";
            acc.LName = "lname";
            acc.UserID = -1;
            Assert.True(await um.UserSignUpAsync(acc));
            Assert.True(await um.AccountExistsAsync(acc.UserID));
            Assert.True(!await _accountAccess.AccountExistsAsync(acc.UserID));
            Assert.True(_accountAccess.NumberOfAccounts());

        }

        [Fact]
        // trys to delete a user, while acting as if an admin executed the command
        public async void DeleteUserSuccess()
        {

            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account admin = new Account();
            admin.Email = "deleteUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";
            admin.IsActive = true;
            admin.UserID = -1;
            await um.UserSignUpAsync(admin);
            await _permissionService.AssignDefaultAdminPermissions(admin.UserID);
            

            Account acc = new Account();
            acc.Email = "deleteUserSuccessEmail";
            acc.Password = "t";
            acc.FName = "fname";
            acc.LName = "lname";
            acc.UserID= -1;
            await um.UserSignUpAsync(acc);
            //Act
            await um.UserDeleteAsync(admin, acc.UserID);
            //Assert
            Assert.True(! await um.AccountExistsAsync(acc.UserID));
            Assert.True(! await _accountAccess.AccountExistsAsync(acc.UserID));
        }
        [Fact]
        public void UpdateSuccess()
        {
            //Arrange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);


            Account admin = new Account();
            admin.Email = "updateUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";
            acc.UserID = -1;
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
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account admin = new Account();
            admin.Email = "disableSuccessAdminEmail";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";
            um.UserSignUp(admin);
            permService.AssignDefaultAdminPermissions(admin.Email);
            
            admin.IsActive = true;

            Account acc = new Account();
            acc.Email = "DisableSuccessUserEmail";
            acc.Password = "t";
            acc.Enabled = true;
            um.UserSignUp(acc);

            //act
            Assert.True(um.DisableAccount(admin, acc.Email));
            //Assert
            acc = accountGateway.GetAccount(acc.Email);
            Assert.True(!acc.Enabled);
            Assert.True(acc.IsActive == false);
        }

        [Fact]
        public void EnableSuccess()
        {
            //Arange 
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account admin = new Account();
            admin.Email = "enableSuccessAdminEmail";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";

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
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

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
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

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
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

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
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account admin = new Account();
            admin.Email = "PromoteToAdminSuccessAdmin";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";

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
