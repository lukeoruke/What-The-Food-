using Console_Runner.AccountService;
using Xunit;

namespace Test.UM
{
    //TODO
    //THESE UNIT TESTS HAVE BEEN MADE SO THAT THEY CAN COMPILE. THEY HAVE NOT BEEN VALIDATED THAT THEY WORK OR TEST FOR THE CORRECT THINGS.
    public class AMRShould
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountGateway _accountAccess = new MemAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new MemAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new MemFlagGateway();
        private readonly IAMRGateway aMRGateway = new MemAMRGateway();

        [Fact]
        public void InstantiateProperly()
        {
            // Arrange
            Account testUser = new Account() { Email = "testaccount@example.com", FName = "test", LName = "account", IsActive = true, Password = "password" , UserID = 12305921};
            Account anotherTestUser = new Account() { Email = "othertestaccount@example.com", FName = "othertest", LName = "account", IsActive = true, Password = "password" , UserID = 128402};

            // Act
            AMR validNonCustomCase = new AMR(true, 100, 630.2001f, 20, ActivityLevel.None);
            AMR validCustomCase = new AMR(true, 40, 230.2001f, 1000, ActivityLevel.Moderate, 200f);

            // Assert

            Assert.True(validNonCustomCase.IsMale);
            Assert.Equal(100, validNonCustomCase.Weight);
            Assert.Equal(630.2001f, validNonCustomCase.Height);
            Assert.Equal(20, validNonCustomCase.Age);
            Assert.Equal(ActivityLevel.None, validNonCustomCase.Activity);
            Assert.False(validNonCustomCase.IsCustomAMR);


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
            Account testUser = new Account() { Email = "testaccount@example.com", FName = "test", LName = "account", IsActive = true, Password = "password" };
            int validWeight = 100;
            int invalidWeight = -10;
            float validHeight = 250f;
            float invalidHeight = -40f;
            int validAge = 45;
            int invalidAge = -22;
            AMR validNonCustomCase = new AMR(true, validWeight, validHeight, validAge, ActivityLevel.Moderate);

            // Act
            validNonCustomCase.Weight = invalidWeight;
            validNonCustomCase.Height = invalidHeight;
            validNonCustomCase.Age = invalidAge;

            // Assert
            // Make sure it actually compares negatives reference off calculate
            Assert.Equal(validWeight, validNonCustomCase.Weight);
            Assert.Equal(validHeight, validNonCustomCase.Height);
            Assert.Equal(validAge, validNonCustomCase.Age);
        }

        //take in actually possible data

        [Fact]
        public void CalculateProperAMR()
        {
            // Arrange
            Account testUser = new Account() { Email = "testaccount@example.com", FName = "test", LName = "account", IsActive = true, Password = "password" };
            int validWeight = 100;
            float validHeight = 630.2001f;
            int validAge = 20;
            ActivityLevel validActivityLevel = ActivityLevel.None;
            AMR nonCustomCase = new AMR(true, validWeight, validHeight, validAge, validActivityLevel);
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
            Account testUser = new Account() { Email = "testaccount@example.com", FName = "test", LName = "account", IsActive = true, Password = "password" };
            AMR customCase = new AMR(true, 40, 230.2001f, 1000, ActivityLevel.Moderate, 200f);
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
