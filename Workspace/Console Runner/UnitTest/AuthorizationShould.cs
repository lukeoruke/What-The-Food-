
using Console_Runner.Account.Account_Implementation_InMemory;
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
        private readonly IAMRGateway _aMRGateway = new MemAMRGateway();
        private readonly IActiveSessionTrackerGateway _activeSessionTrackerGateway = new MemActiveSessionTracker();

        [Fact]
        public void InstantiateProperly()
        {

            // Arrange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _aMRGateway, _activeSessionTrackerGateway);
            int id = 91929821;
            string testPerm = "scanFood";

            // Act
            Authorization perm1 = new Authorization(testPerm);
            perm1.UserID = id;

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
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _aMRGateway, _activeSessionTrackerGateway);
            int testID = 12312;
            string testResource = "scanFood";
            int AdminID = 446546;
            string AdminResource = "deleteAccount";

            // Act
            Authorization perm1 = new Authorization(testResource);
            perm1.UserID = testID;
       
            Authorization perm2 = new Authorization(AdminResource);
            perm2.UserID = AdminID;


            // Assert
            Assert.NotNull(perm1);
            Assert.True(perm1.UserID == testID);
            Assert.True(perm1.Permission == testResource);
            Assert.True(perm2.UserID == AdminID);
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
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _aMRGateway, _activeSessionTrackerGateway);
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

            Assert.True(await um.HasPermissionAsync(acc.UserID, "scanFood"));
            Assert.True(await um.HasPermissionAsync(acc.UserID, "editOwnAccount"));
            Assert.True(await um.HasPermissionAsync(acc.UserID, "leaveReview"));
            Assert.True(await um.HasPermissionAsync(acc.UserID, "deleteOwnAccount"));
            Assert.True(await um.HasPermissionAsync(acc.UserID, "historyAccess"));
            Assert.True(await um.HasPermissionAsync(acc.UserID, "AMR"));
            Assert.True(await um.HasPermissionAsync(acc.UserID, "foodFlag"));
        }

        [Fact]
        public async void SetAdminPermissionsProperly()
        {
            // Arrange
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _aMRGateway, _activeSessionTrackerGateway);
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
