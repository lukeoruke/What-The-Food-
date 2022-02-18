using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Console_Runner;
using Console_Runner.AMR;

namespace UnitTest
{
    public class AMRShould
    {
        [Fact]
        public void InstantiateProperly()
        {
            // Arrange

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
            Assert.Equal(validWeight, validNonCustomCase.Weight);
            Assert.Equal(validHeight, validNonCustomCase.Height);
            Assert.Equal(validAge, validNonCustomCase.Age);
        }

        [Fact]
        public void CalculateProperAMR()
        {
            // Arrange
            AMR nonCustomCase = new AMR(true, 100, 630.2001f, 20, ActivityLevel.None);
            float nonCustomAMR = (66.47f + (13.75f * 100) + (5.003f * 630.2001f) - (6.755f * 20)) * 1.2f;

            // Act
            float returnedNonCustomAMR = nonCustomCase.CalculateAMR();

            // Assert
            Assert.Equal(nonCustomAMR, returnedNonCustomAMR);
        }

        [Fact]
        public void ReturnCustomAMRWhenAppropriate()
        {
            // Arrange
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
