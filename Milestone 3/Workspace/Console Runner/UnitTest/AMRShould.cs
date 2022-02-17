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
            AMR negativeValueCase = new AMR(false, -10, 22.1f, 15, ActivityLevel.Heavy);

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
    }
}
