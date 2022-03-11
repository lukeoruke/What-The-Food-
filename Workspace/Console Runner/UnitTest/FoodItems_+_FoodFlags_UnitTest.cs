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
        public void AddFoodFlagSuccess()
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
            Assert.True(fm.AddFlagToAccount("Matt@gmail.com", "1"));
            Assert.True(fm.GetAllAccountFlags("Tyler@gmail.com").Count == 1);
        }


        [Fact]
        public void RemoveFoodFlagSuccess()
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
            Assert.True(fm.AddFlagToAccount("Tyler@gmail.com", "1"));
            Assert.True(fm.GetAllAccountFlags("Tyler@gmail.com").Count == 1);
            Assert.True(fm.accountHasFlag("Tyler@gmail.com", "1"));
            Assert.True(fm.RemoveFoodFlag("Tyler@gmail.com", "1"));
            Assert.True(fm.GetAllAccountFlags("Tyler@gmail.com").Count == 0);
        }
        [Fact]
        public void accountHasFlagSuccess()
        {
            IFoodItemGateway foodItemGateway = new MemFoodItemGateway();
            IFlagGateway flagGateway = new MemFlagGateway();
            IlogGateway logGateway = new MemLogGateway();
            FM fm = new FM(foodItemGateway, flagGateway, logGateway);
            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            ingredient.IngredientID = "2";
            Assert.False(fm.accountHasFlag("Luke@gmail.com", "2"));
            Assert.True(fm.AddFlagToAccount("Luke@gmail.com", "2"));
            Assert.True(fm.accountHasFlag("Luke@gmail.com", "2"));
        }
        [Fact]
        public void getAllAccountFlagsSuccess()
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
            Ingredient ingredient2 =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            ingredient.IngredientID = "2";
            Ingredient ingredient3 =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            ingredient.IngredientID = "3";

            fm.AddFlagToAccount("MrHas3Flags@gmail.com", "1");
            fm.AddFlagToAccount("MrHas3Flags@gmail.com", "2");
            fm.AddFlagToAccount("MrHas3Flags@gmail.com", "3");

            fm.AddFlagToAccount("MrHas1Flag@gmail.com", "1");

            Assert.False(fm.GetAllAccountFlags("mrHasNoFlags@gmail.com").Count == 1);
            Assert.True(fm.GetAllAccountFlags("mrHasNoFlags@gmail.com").Count == 0);
            Assert.True(fm.GetAllAccountFlags("MrHas3Flags@gmail.com").Count == 3);
            Assert.False(fm.GetAllAccountFlags("MrHas3Flags@gmail.com").Count == 4);
            Assert.False(fm.GetAllAccountFlags("MrHas3Flags@gmail.com").Count == 2);
            Assert.True(fm.GetAllAccountFlags("MrHas1Flag@gmail.com").Count == 1);
        }


        [Fact]
        public void AddFoodSuccess()
        {
            IFoodItemGateway foodItemGateway = new MemFoodItemGateway();
            IFlagGateway flagGateway = new MemFlagGateway();
            IlogGateway logGateway = new MemLogGateway();
            FM fm = new FM(foodItemGateway, flagGateway, logGateway);

            FoodItem food = new FoodItem("70847-81116", "Monster Energy Drink", "CocaCola Co");


            NutritionLabel nutrition = new NutritionLabel(270, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, new List<Nutrient>(), "70847-81116");

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
