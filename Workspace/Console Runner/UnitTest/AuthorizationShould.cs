
using Console_Runner.AccountService;
using Xunit;
namespace Test.UM
{
    public class AuthorizationShould
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountGateway _accountAccess = new MemAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new MemAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new MemFlagGateway();

        [Fact]
        public void InstantiateProperly()
        {

            // Arrange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            int id = 91929821;
            string testPerm = "scanFood";

            // Act
            Authorization perm1 = new Authorization(id, testPerm);

            // Assert
            Assert.NotNull(perm1);
            Assert.True(perm1.UserID != 0);
            Assert.NotNull(perm1.Permission);
            Assert.Equal(id, perm1.UserID);
            Assert.Equal(testPerm, perm1.Permission);
        }

        [Fact]
        public void SetPropertiesProperly()
        {
            // Arrange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            int testID = 12312;
            string testResource = "scanFood";
            int AdminID = 446546;
            string AdminResource = "deleteAccount";

            // Act
            Authorization perm1 = new Authorization(testID, testResource);
       
            Authorization perm2 = new Authorization(AdminID, AdminResource);


            // Assert
            Assert.NotNull(perm1);
            Assert.True(perm1.UserID == testID);
            Assert.True(perm1.Permission == testResource);
            Assert.True(perm2.UserID == testID);
            Assert.True(perm2.Permission == AdminResource);
            Assert.NotNull(perm1.Permission);
            Assert.NotNull(perm1.Permission);
            Assert.NotNull(perm2.Permission);
            Assert.NotNull(perm2.Permission);
            Assert.NotEqual(AdminID, perm1.UserID);
            Assert.NotEqual(AdminResource, perm1.Permission);
        }

        [Fact]
        public async void SetDefaultPermissionsProperly()
        {
            // Arrange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            string tester = "SetDefaultPermissionsProperly";
            Account acc = new Account();
            acc.Email = tester;
            acc.UserID = 509123;

            // Act

            await _permissionService.AssignDefaultUserPermissions(acc.UserID);

            // Assert
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "scanFood"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "editOwnAccount"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "leaveReview"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "deleteOwnAccount"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "historyAccess"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "AMR"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "foodFlag"));
        }

        [Fact]
        public async void SetAdminPermissionsProperly()
        {
            // Arrange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            string tester = "SetAdminPermissionsProperly";
            Account acc = new Account();
            acc.Email = tester;

            acc.UserID = 509123;

            // Act

            await _permissionService.AssignDefaultAdminPermissions(acc.UserID);

            // Assert
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "enableAccount"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "disableAccount"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "deleteAccount"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "createAdmin"));
            Assert.True(await _permissionService.HasPermissionAsync(acc.UserID, "editOtherAccount"));
        }


    }
}
