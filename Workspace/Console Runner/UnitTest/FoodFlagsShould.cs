using Console_Runner.AccountService;
using Console_Runner.FoodService;
using System;
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
        private Random _random = new Random();
        [Fact]
        public async void AddFoodFlagSuccess()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            Account acc = new Account();
            await um.UserSignUpAsync(acc);
            Assert.True(await um.AddFlagToAccountAsync(acc.UserID, ingredient.IngredientID));
            Assert.True(await _flagGateway.AccountHasFlagAsync(acc.UserID, ingredient.IngredientID));
            Assert.True((await _flagGateway.GetAllAccountFlagsAsync(acc.UserID)).Count == 1);
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
            await fm.AddIngredientAsync(ingredient);
            int userID = _random.Next();
            Assert.True(await um.AddFlagToAccountAsync(userID, ingredient.IngredientID));
            Assert.True((await um.GetAllAccountFlagsAsync(userID)).Count == 1);
            Assert.True(await um.accountHasFlagAsync(userID, ingredient.IngredientID));
            Assert.True(await um.RemoveFoodFlagAsync(userID, ingredient.IngredientID));
            Assert.True((await um.GetAllAccountFlagsAsync(userID)).Count == 0);
        }
        [Fact]
        public async void accountHasFlagSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            int userID = _random.Next();
            Assert.False(await um.accountHasFlagAsync(userID, ingredient.IngredientID));
            Assert.True(await um.AddFlagToAccountAsync(userID, ingredient.IngredientID));
            Assert.True(await um.accountHasFlagAsync(userID, ingredient.IngredientID));
        }
        [Fact]
        public async void getAllAccountFlagsSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            Ingredient ingredient2 =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            Ingredient ingredient3 =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            int has3Flags = _random.Next();
            int has1Flag = _random.Next();
            int has0Flags = _random.Next();
            await um.AddFlagToAccountAsync(has3Flags, ingredient.IngredientID);
            await um.AddFlagToAccountAsync(has3Flags, ingredient2.IngredientID);
            await um.AddFlagToAccountAsync(has3Flags, ingredient3.IngredientID);
            await um.AddFlagToAccountAsync(has1Flag, ingredient2.IngredientID);

            Assert.False((await um.GetAllAccountFlagsAsync(has3Flags)).Count == 1);
            Assert.True((await um.GetAllAccountFlagsAsync(has0Flags)).Count == 0);
            Assert.True((await um.GetAllAccountFlagsAsync(has3Flags)).Count == 3);
            Assert.False((await um.GetAllAccountFlagsAsync(has3Flags)).Count == 4);
            Assert.False((await um.GetAllAccountFlagsAsync(has3Flags)).Count == 2);
            Assert.True((await um.GetAllAccountFlagsAsync(has1Flag)).Count == 1);
        }
        [Fact]
        public async void GetNFlagsSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            Ingredient ingredient2 =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            Ingredient ingredient3 =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            int someUser = 101012219;
            await um.AddFlagToAccountAsync(someUser, ingredient.IngredientID);
            await um.AddFlagToAccountAsync(someUser, ingredient2.IngredientID);
            await um.AddFlagToAccountAsync(someUser, ingredient3.IngredientID);
            Assert.True((await um.GetNAccountFlagsAsync(someUser, 0, 1)).Count == 1);

            Assert.True((await um.GetNAccountFlagsAsync(someUser, 0, 2)).Count == 2);

            Assert.True((await um.GetNAccountFlagsAsync(someUser, 0, 3)).Count == 3);

            Assert.True((await um.GetNAccountFlagsAsync(someUser, 1, 2)).Count == 2);
        }


        //TODO IMPLEMENT THIS

/*        [Fact]
        public void checkProductForFlags()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway);
            AccountDBOperations um = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = _random.Next();

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
        }*/


    }
}



