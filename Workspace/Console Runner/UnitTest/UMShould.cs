using Xunit;
using Console_Runner.AccountService;
using System;

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
            Assert.True(acc.UserID == 0);
            Assert.True(await um.UserSignUpAsync(acc));
            
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(tester));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "scanFood"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "editOwnAccount"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "leaveReview"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "deleteOwnAccount"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "historyAccess"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "AMR"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "foodFlag"));
            Assert.True(await um.AccountExistsAsync(acc.UserID));
            Assert.True(await _accountAccess.AccountExistsAsync(acc.UserID));
            Assert.True(_accountAccess.NumberOfAccounts() == 1);



            


        }

        [Fact]
        // trys to delete a user, while acting as if an admin executed the command
        public async void DeleteUserSuccess()
        {

            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account admin = new Account();
            admin.Email = "deleteUserSuccessAdminEmailvda";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";
            await um.UserSignUpAsync(admin);
            admin = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(admin.Email));
            admin.IsActive = true;
            Assert.True(await _permissionService.AssignDefaultAdminPermissions(admin.UserID));


            

            Account acc = new Account();
            acc.Email = "deleteUserSuccessEmaildd";
            acc.Password = "dsst";
            acc.FName = "fname";
            acc.LName = "lname";
            await um.UserSignUpAsync(acc);
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(acc.Email));
            //Act

            //Assert
            Assert.True(await um.UserDeleteAsync(admin, acc.UserID));
            //Assert
            Assert.True(! await um.AccountExistsAsync(acc.UserID));
            Assert.True(! await _accountAccess.AccountExistsAsync(acc.UserID));
        }
        [Fact]
        public async void UpdateSuccess()
        {
            //Arrange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);


            Account admin = new Account();
            admin.Email = "updateUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";
            
            await um.UserSignUpAsync(admin);
            admin = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(admin.Email));
            await _permissionService.AssignDefaultAdminPermissions(admin.UserID);
            admin.IsActive = true;


            Account acc = new Account();
            acc.Email = "updateUserSuccessEmail";
            acc.Password = "t";
            await um.UserSignUpAsync(acc);
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(acc.Email));
            string nName = "new name";
            string nlName = "new last name";
            string nPassword = "NewPassword";

            //act
            await um.UserUpdateDataAsync(admin, acc.UserID, nName, nlName, nPassword);
            acc = await _accountAccess.GetAccountAsync(acc.UserID);
            //Assert
            Assert.True(acc.FName == nName && acc.LName == nlName && acc.Password == nPassword);
            Assert.True(acc.IsActive == false);
        }
        [Fact]
        public async void DisableSuccess()
        {
            //arange 
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account admin = new Account();
            admin.Email = "disableSuccessAdminEmail";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";
            await um.UserSignUpAsync(admin);
            admin = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(admin.Email));
            await _permissionService.AssignDefaultAdminPermissions(admin.UserID);


            admin.IsActive = true;

            Account acc = new Account();
            acc.Email = "DisableSuccessUserEmail";
            acc.Password = "t";
            acc.Enabled = true;
            await um.UserSignUpAsync(acc);
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(acc.Email));
            Assert.True(acc.Enabled);
            //act
            Assert.True(await um.DisableAccountAsync(admin, acc.UserID));
            //Assert
            acc = await _accountAccess.GetAccountAsync(acc.UserID);
            Assert.True(!acc.Enabled);
            Assert.True(acc.IsActive == false);
        }

        [Fact]
        public async void EnableSuccess()
        {
            //Arange 
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account admin = new Account();
            admin.Email = "enableSuccessAdminEmail";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";

            Assert.True(await um.UserSignUpAsync(admin));
            admin = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(admin.Email));
            admin.IsActive = true;
            Assert.True(await _permissionService.AssignDefaultAdminPermissions(admin.UserID));
            Assert.True(admin.IsActive);

            Account acc = new Account();
            acc.Email = "enableeSuccessUserEmail";
            acc.Password = "tdsfea";
            acc.FName = "fname";
            acc.LName = "lname";
            Assert.True(await um.UserSignUpAsync(acc));
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(acc.Email));
            acc.Enabled = false;
            Assert.True(acc.Enabled == false);
            //act
            Assert.True(await um.EnableAccountAsync(admin, acc.UserID));
            //Assert
            Assert.True(acc.Enabled);
        }




        [Fact]
        public async void GetUserSuccess()
        {
            //arange 
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account acc = new Account();
            acc.Email = "getUserSuccess";
            acc.Password = "t";
            acc.Enabled = false;
            await um.UserSignUpAsync(acc);
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(acc.Email));
            //act
            Account temp = await um.GetUserAccountAsync(acc.UserID);

            //Assert
            Assert.True(temp != null);
            Assert.True(temp == acc);
        }
        [Fact]
        public async void AuthenticatePasswordSuccess()
        {
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account acc = new Account();
            acc.Email = "authenticatePasswordSuccess";
            acc.Password = "password!";
            await um.UserSignUpAsync(acc);
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(acc.Email));

            //Assert
            Assert.True(await um.AuthenticateUserPassAsync(acc.Email, acc.Password));
            Assert.False(await um.AuthenticateUserPassAsync(acc.Email, "t"));
        }
        [Fact]
        public async void SignInSuccess()
        {
            //arange 
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account acc = new Account();
            acc.Email = "signInSuccess";
            acc.Password = "pass";
            await um.UserSignUpAsync(acc);
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(acc.Email));
            //act
            Assert.True(await um.SignInAsync(acc.Email,acc.Password) != null);
            //Assert
            Assert.True(acc.IsActive);
        }
        [Fact]
        public async void PromoteToAdminSuccess()
        {
            //arange 
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Account admin = new Account();
            admin.Email = "PromoteToAdminSuccessAdmin";
            admin.Password = "password1";
            admin.FName = "fname";
            admin.LName = "lname";

            
            await um.UserSignUpAsync(admin);
            admin = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(admin.Email));
            await _permissionService.AssignDefaultAdminPermissions(admin.UserID);
            admin.IsActive = true;

            Account acc = new Account();
            acc.Email = "PromoteToAdminSuccessss";
            acc.Password = "pass";
            acc.FName = "esfs";
            acc.LName = "asda";
            Assert.True(await um.UserSignUpAsync(acc));
            acc = await _accountAccess.GetAccountAsync(_accountAccess.GetIDFromEmail(acc.Email));
            
            //act
            Assert.True(await um.PromoteToAdmin(admin, acc.UserID));

            //Assert
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "createAdmin"));
            Assert.True(um.IsAdmin(acc.UserID));
        }

    }
}
