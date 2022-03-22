using Console_Runner.AccountService;
using Console_Runner.FoodService;
using System.Collections.Generic;
using Xunit;

namespace Test.UM
{
    public class FoodFlagsShould
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountGateway _accountAccess = new MemAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new MemAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new MemFlagGateway();
        private readonly IFoodGateway _foodGateway = new MemFoodGateway();
        [Fact]
        public async void AddFoodFlagSuccess()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            ingredient.IngredientID = "1";
            Assert.True(await um.AddFlagToAccountAsync(1532, 1));

            //Assert.True(fm.GetAllAccountFlagsAsync("Matt@gmail.com").Count == 1);
        }


        [Fact]
        public async void RemoveFoodFlagSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            ingredient.IngredientID = "1";
            Assert.True(await um.AddFlagToAccountAsync(55, 56));
            Assert.True(um.GetAllAccountFlagsAsync("Tyler@gmail.com").Count == 1);
            Assert.True(um.accountHasFlagAsync("Tyler@gmail.com", "1"));
            Assert.True(um.RemoveFoodFlagAsync("Tyler@gmail.com", "1"));
            Assert.True(um.GetAllAccountFlagsAsync("Tyler@gmail.com").Count == 0);
        }
        [Fact]
        public void accountHasFlagSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
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
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
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
        public void checkProductForFlags()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = "128";

            FoodItem foodItem = new("701231847-841411116", "Monster Energy Drink", "CokaCola Co");
            LabelIdentifier labelIdentifier = new LabelIdentifier(foodItem.Barcode, "128");
            Nutrient nutrient = new Nutrient(foodItem.Barcode, "SomethingHealthy");
            List<Nutrient> nutritionList = new();
            nutritionList.Add(nutrient);

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            fm.AddFoodItem(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList);
            fm.AddFlagToAccount("dudewithaflag@gmail.com", "128");
            List<Ingredient> flaggedItemsInGivenFood = fm.CheckProductForFlags(foodItem.Barcode, "dudewithaflag@gmail.com");
            Assert.True(flaggedItemsInGivenFood.Count > 0);
            Assert.True(flaggedItemsInGivenFood[0].Equals(ingredient));
        }


    }
}



