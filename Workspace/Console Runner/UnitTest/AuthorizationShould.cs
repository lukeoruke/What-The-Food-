using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Console_Runner;
using Console_Runner.DAL;
using Console_Runner.User_Management;

namespace UnitTest
{
    public class AuthorizationShould
    {
        [Fact]
        public void InstantiateProperly()
        {

            // Arrange
            IPermissionGateway EfPermission = new MemPermissionGateway();
            string testEmail = "test@example.com";
            string testPerm = "scanFood";

            // Act
            Permission perm1 = new Permission(testEmail, testPerm);

            // Assert
            Assert.NotNull(perm1);
            Assert.NotNull(perm1.Email);
            Assert.NotNull(perm1.Resource);
            Assert.Equal(testEmail, perm1.Email);
            Assert.Equal(testPerm, perm1.Resource);
        }

        [Fact]
        public void SetPropertiesProperly()
        {
            // Arrange
            IPermissionGateway EfPermission = new MemPermissionGateway();
            string testEmail = "test@example.com";
            string testResource = "scanFood";
            string modEmail = "test1@example.com";
            string modResource = "deleteAccount";

            // Act
            Permission perm1 = new Permission(testEmail, testResource);
            EfPermission.AddPermission(perm1);

            // Assert
            Assert.NotNull(perm1);
            Assert.NotNull(perm1.Email);
            Assert.NotNull(perm1.Resource);
            Assert.NotEqual(modEmail, perm1.Email);
            Assert.NotEqual(modResource, perm1.Resource);
        }

        [Fact]
        public void SetDefaultPermissionsProperly()
        {
            // Arrange
            IPermissionGateway EfPermission = new MemPermissionGateway();
            string testEmail = "test@example.com";

            // Act
            PermissionService permService = new PermissionService(EfPermission);
            permService.AssignDefaultUserPermissions(testEmail);
;

            // Assert
            Assert.True(EfPermission.HasPermission(testEmail, "scanFood"));
            Assert.True(EfPermission.HasPermission(testEmail, "editOwnAccount"));
            Assert.True(EfPermission.HasPermission(testEmail, "leaveReview"));
            Assert.True(EfPermission.HasPermission(testEmail, "deleteOwnAccount"));
            Assert.True(EfPermission.HasPermission(testEmail, "historyAccess"));
            Assert.True(EfPermission.HasPermission(testEmail, "AMR"));
            Assert.True(EfPermission.HasPermission(testEmail, "foodFlag"));
        }

        [Fact]
        public void SetAdminPermissionsProperly()
        {
            // Arrange
            IPermissionGateway EfPermission = new MemPermissionGateway();
            string testEmail = "test@example.com";

            // Act
            PermissionService permService = new PermissionService(EfPermission);
            permService.AssignDefaultAdminPermissions(testEmail);

            // Assert
            Assert.True(EfPermission.HasPermission(testEmail, "enableAccount"));
            Assert.True(EfPermission.HasPermission(testEmail, "disableAccount"));
            Assert.True(EfPermission.HasPermission(testEmail, "deleteAccount"));
            Assert.True(EfPermission.HasPermission(testEmail, "createAdmin"));
            Assert.True(EfPermission.HasPermission(testEmail, "editOtherAccount"));
        }


    }
}
