using Console_Runner.DAL;
using Console_Runner.Food;
using Food_Class_Library;
using System.Collections.Generic;
using Xunit;

namespace UnitTest
{
    public class FoodItems___FoodFlags_UnitTest
    {
        [Fact]
        public void addFlagToAccountSuccess()
        {
            IFoodItemGateway foodItemGateway = new MemFoodItemGateway();
            IFlagGateway flagGateway = new MemFlagGateway();
            IlogGateway logGateway = new MemLogGateway();
            FM fm = new FM(foodItemGateway, flagGateway, logGateway);

            FoodItem food = new FoodItem("70847-81116", "Monster Energy Drink", "CocaCola Co");


            NutritionLabel nutrition = new NutritionLabel(230, 1, 1, 0, 180, 0, 0, 180, 27, 0, 27, 27, 0, "70847-81116");

            Ingredient ingredient =
                new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
                "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            ingredient.IngredientID = "1";
            LabelIdentifier label = new LabelIdentifier("70847-81116", "1");
            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);

            fm.AddFoodItem(food, nutrition, new List<Nutrient>(), ingredientList);

            Assert.True(fm.AddFlagToAccount("Matt@gmail.com", "1"));
            List<Ingredient> flaggedItems = fm.CheckProductForFlags("Matt@gmail.com", "70847-81116");
            Assert.True(flaggedItems.Count == 1);
            Assert.True(flaggedItems[0].Equals(ingredient));


        }
    }
}
