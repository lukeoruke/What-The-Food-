using Console_Runner.DAL;
using Console_Runner.Food;
using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class FoodItemsShould
    {
        [Fact]
        public void AddFoodSuccess()
        {
            IFoodItemGateway foodItemGateway = new MemFoodItemGateway();
            IFlagGateway flagGateway = new MemFlagGateway();
            IlogGateway logGateway = new MemLogGateway();
            FM fm = new FM(foodItemGateway, flagGateway, logGateway);
            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            ingredient.IngredientID = "1";

            FoodItem foodItem = new FoodItem("70847-81116", "Monster Energy Drink", "CokaCola Co");
            LabelIdentifier labelIdentifier = new LabelIdentifier("70847-81116", "1");
            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, new List<Nutrient>(), "70847-81116");
            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            Assert.True(fm.AddFoodItem(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList));
       
        }
    }
}
  /*   LabelIdentifier label = new LabelIdentifier("70847-81116", "1");

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);



            Assert.True(fm.AddFlagToAccount("Matt@gmail.com", "1"));
            List<Ingredient> flaggedItems = fm.CheckProductForFlags("Matt@gmail.com", "70847-81116");
            Assert.True(flaggedItems.Count == 1);
            Assert.True(flaggedItems[0].Equals(ingredient));
            Assert.True(true);*/