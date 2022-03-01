using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Console_Runner;
using Console_Runner.AMRModel;
using User;

namespace UnitTest
{
    public class AMRShould
    {
        [Fact]
        public void InstantiateProperly()
        {
            // Arrange
            Account testUser = new Account() { Email = "testaccount@example.com", Fname = "test", Lname = "account", isActive = true, Password = "password" };
            Account anotherTestUser = new Account() { Email = "othertestaccount@example.com", Fname = "othertest", Lname = "account", isActive = true, Password = "password" };

            // Act
            AMR validNonCustomCase = new AMR(testUser, true, 100, 630.2001f, 20, ActivityLevel.None);
            AMR validCustomCase = new AMR(anotherTestUser, true, 40, 230.2001f, 1000, ActivityLevel.Moderate, 200f);

            // Assert
            Assert.Equal(testUser, validNonCustomCase.Account);
            Assert.True(validNonCustomCase.IsMale);
            Assert.Equal(100, validNonCustomCase.Weight);
            Assert.Equal(630.2001f, validNonCustomCase.Height);
            Assert.Equal(20, validNonCustomCase.Age);
            Assert.Equal(ActivityLevel.None, validNonCustomCase.Activity);
            Assert.False(validNonCustomCase.IsCustomAMR);

            Assert.Equal(anotherTestUser, validCustomCase.Account);
            Assert.True(validCustomCase.IsMale);
            Assert.Equal(40, validCustomCase.Weight);
            Assert.Equal(230.2001f, validCustomCase.Height);
            Assert.Equal(1000, validCustomCase.Age);
            Assert.Equal(ActivityLevel.Moderate, validCustomCase.Activity);
            Assert.True(validCustomCase.IsCustomAMR);
            Assert.Equal(200f, validCustomCase.CustomAMR);
        }

        [Fact]
        public void RejectNegativeMetrics()
        {
            // Arrange
            Account testUser = new Account() { Email = "testaccount@example.com", Fname = "test", Lname = "account", isActive = true, Password = "password" };
            int validWeight = 100;
            int invalidWeight = -10;
            float validHeight = 250f;
            float invalidHeight = -40f;
            int validAge = 45;
            int invalidAge = -22;
            AMR validNonCustomCase = new AMR(testUser, true, validWeight, validHeight, validAge, ActivityLevel.Moderate);

            // Act
            validNonCustomCase.Weight = invalidWeight;
            validNonCustomCase.Height = invalidHeight;
            validNonCustomCase.Age = invalidAge;

            // Assert
            Assert.Equal(validWeight, validNonCustomCase.Weight);
            Assert.Equal(validHeight, validNonCustomCase.Height);
            Assert.Equal(validAge, validNonCustomCase.Age);
        }

        [Fact]
        public void CalculateProperAMR()
        {
            // Arrange
            Account testUser = new Account() { Email = "testaccount@example.com", Fname = "test", Lname = "account", isActive = true, Password = "password" };
            int validWeight = 100;
            float validHeight = 630.2001f;
            int validAge = 20;
            ActivityLevel validActivityLevel = ActivityLevel.None;
            AMR nonCustomCase = new AMR(testUser, true, validWeight, validHeight, validAge, validActivityLevel);
            float nonCustomAMR = ((10 * validWeight) + (6.25f * validHeight) - (5 * validAge) + 5) * 1.2f;

            // Act
            float returnedNonCustomAMR = nonCustomCase.CalculateAMR();

            // Assert
            Assert.Equal(nonCustomAMR, returnedNonCustomAMR);
        }

        [Fact]
        public void ReturnCustomAMRWhenAppropriate()
        {
            // Arrange
            Account testUser = new Account() { Email = "testaccount@example.com", Fname = "test", Lname = "account", isActive = true, Password = "password" };
            AMR customCase = new AMR(testUser, true, 40, 230.2001f, 1000, ActivityLevel.Moderate, 200f);
            float nonCustomAMR = (66.47f + (13.75f * 40) + (5.003f * 230.2001f) - (6.755f * 1000)) * 1.55f;
            float customAMR = 200f;

            // Act
            float returnedCustomAMR = customCase.CalculateAMR();

            // Assert
            Assert.NotEqual(nonCustomAMR, returnedCustomAMR);
            Assert.Equal(customAMR, returnedCustomAMR);
        }
    }
}
