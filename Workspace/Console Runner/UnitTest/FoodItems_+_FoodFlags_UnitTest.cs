using Console_Runner.DAL;
using Console_Runner.Food;
using LogAndArchive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class FoodItems___FoodFlags_UnitTest
    {
        [Fact]
        public void addFlagToAccountSuccess()
        {
            //Arrange
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            FM fm = new FM(dal, log);
            Ingredient ingredient = new Ingredient("Caffiene", "NA", "Description here");
            //act
            fm.addIngredient(ingredient);
            fm.addFlagToAccount("email", ingredient)
            //Assert
            Assert.True(dal.accountHasFlag(ingredient));
            

        }
    }
}
