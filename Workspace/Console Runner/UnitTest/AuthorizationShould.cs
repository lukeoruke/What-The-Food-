using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Console_Runner;
using Console_Runner.DAL;

namespace UnitTest
{
    public class AuthorizationShould
    {
        [Fact]
        public void InstantiateProperly()
        {
            // Arrange
            IDataAccess dal = new DummyDaL();
            string testEmail = "test@example.com";
            string testPerm = "scanFood";

            // Act
            user_permissions perm1 = new user_permissions(testEmail, testPerm, dal);

            // Assert
            Assert.NotNull(perm1);
            Assert.NotNull(perm1.email);
            Assert.NotNull(perm1.permission);
            Assert.Equal(testEmail, perm1.email);
            Assert.Equal(testPerm, perm1.permission);
        }

        [Fact]
        public void SetPropertiesProperly()
        {
            // Arrange
            IDataAccess dal = new DummyDaL();
            string testEmail = "test@example.com";
            string testPerm = "scanFood";
            string modEmail = "test1@example.com";
            string modPerm = "deleteAccount";

            // Act
            user_permissions perm1 = new user_permissions(testEmail, testPerm, dal);
            perm1.setUserPermissions(modEmail, modPerm);

            // Assert
            Assert.NotNull(perm1);
            Assert.NotNull(perm1.email);
            Assert.NotNull(perm1.permission);
            Assert.Equal(modEmail, perm1.email);
            Assert.Equal(modPerm, perm1.permission);
        }

        [Fact]
        public void SetDefaultPermissionsProperly()
        {
            // Arrange
            IDataAccess dal = new DummyDaL();
            string testEmail = "test@example.com";

            // Act
            user_permissions perm1 = new user_permissions("", "", dal);
            perm1.AssignDefaultUserPermissions(testEmail);

            // Assert
            Assert.True(dal.hasPermission(testEmail, "scanFood"));
            Assert.True(dal.hasPermission(testEmail, "editOwnAccount"));
            Assert.True(dal.hasPermission(testEmail, "leaveReview"));
            Assert.True(dal.hasPermission(testEmail, "deleteOwnAccount"));
            Assert.True(dal.hasPermission(testEmail, "historyAccess"));
            Assert.True(dal.hasPermission(testEmail, "AMR"));
            Assert.True(dal.hasPermission(testEmail, "foodFlag"));
        }

        [Fact]
        public void SetAdminPermissionsProperly()
        {
            // Arrange
            IDataAccess dal = new DummyDaL();
            string testEmail = "test@example.com";

            // Act
            user_permissions perm1 = new user_permissions("", "", dal);
            perm1.defaultAdminPermissions(testEmail);

            // Assert
            Assert.True(dal.hasPermission(testEmail, "enableAccount"));
            Assert.True(dal.hasPermission(testEmail, "disableAccount"));
            Assert.True(dal.hasPermission(testEmail, "deleteAccount"));
            Assert.True(dal.hasPermission(testEmail, "createAdmin"));
            Assert.True(dal.hasPermission(testEmail, "editOtherAccount"));
        }


    }
}
