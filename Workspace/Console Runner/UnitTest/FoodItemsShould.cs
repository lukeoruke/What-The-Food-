using Console_Runner.FoodService;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.FM
{
    public class FoodItemsShould
    {
        private Random _random = new Random();
        private readonly IFoodGateway _foodGateway = new MemFoodGateway();
        private readonly IFoodUpdateGateway _foodUpdateGateway = new MemFoodUpdateGateway();
        [Fact]
        public async void AddNewProductSuccess()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = _random.Next();
            string barcode = _random.Next().ToString();
            FoodItem foodItem = new(barcode, "Monster Energy Drink", "CokaCola Co", "r/");
            Nutrient nutrient = new Nutrient("SomethingHealthy");
            nutrient.NutrientID = _random.Next();
            LabelIngredient ingredientIdentifier = new LabelIngredient(barcode, ingredient.IngredientID);
            LabelNutrient nutrientIdentifier = new LabelNutrient(barcode, nutrient.NutrientID, 0.14f);

            List<(Nutrient, float)> nutritionList = new();
            nutritionList.Add((nutrient, 0.1f));

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            Assert.True(await fm.AddNewProductAsync(foodItem, nutritionLabel, ingredientList));
        }
        [Fact]
        public async void GetScannedFoodItemSuccess()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);


            string barcode = "lao109341";
            FoodItem foodItem = new(barcode, "Monster Energy Drink", "CokaCola Co", "r/");
            fm.AddFoodItemAsync(foodItem);
            FoodItem? returnedFoodItem = await fm.GetScannedItemAsync(foodItem.Barcode);
            Assert.True(returnedFoodItem != null);
            Assert.True(returnedFoodItem?.Equals(foodItem));

        }

        [Fact]
        public async void GetNutritionLabelSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);





            FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co", "/rDir");
            Nutrient nutrient = new Nutrient("SomethingHealthy");
            await fm.AddNutrientAsync(nutrient);
            List<(Nutrient, float)> nutritionList = new();
            nutritionList.Add((nutrient, 0.23f));
            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);
            await fm.AddNutritionLabelAsync(nutritionLabel);


            NutritionLabel? returnedLabel = await fm.GetNutritionLabelAsync(foodItem.Barcode);
            Assert.True(returnedLabel?.Equals(nutritionLabel));
        }
        [Fact]
        public async void GetIngredientList()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);

            Ingredient ingredient2 =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            Ingredient ingredient3 =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");



            FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co", "r/asda");
            LabelIngredient labelIdentifier = new LabelIngredient(foodItem.Barcode, ingredient.IngredientID);
            Nutrient nutrient = new Nutrient("SomethingHealthy");
            fm.AddNutrientAsync(nutrient);
            List<(Nutrient, float)> nutritionList = new();
            nutritionList.Add((nutrient, 0.5f));

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            ingredientList.Add(ingredient2);
            ingredientList.Add(ingredient3);
            await fm.AddNewProductAsync(foodItem, nutritionLabel, ingredientList);
            List<Ingredient> ingredientListRetrieved = await fm.GetIngredientsListAsync(foodItem.Barcode);
            Assert.True(ingredientListRetrieved.Count > 0);
            Assert.True(ingredientListRetrieved[0].Equals(ingredient));
        }
        [Fact]
        public async void GetIngredient()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);

            Ingredient ingredient = new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water "
                + "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            Ingredient ingredient2 =
                new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
                "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            await fm.AddIngredientAsync(ingredient);
            await fm.AddIngredientAsync(ingredient2);

            Assert.True((await fm.GetIngredient(ingredient.IngredientID)).Equals(ingredient));
            Assert.False((await fm.GetIngredient(ingredient.IngredientID)).Equals(ingredient2));
            Assert.True((await fm.GetIngredient(ingredient2.IngredientID)).Equals(ingredient2));

        }
        [Fact]
        public async void GeIngredientBySearchAsyncSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);

            Ingredient ingredient = new Ingredient("AAAAAAAAAA", "bubbly water", "Carbonated water is water " 
                + "containing dissolved carbon dioxide gas, either artificially injected under " + 
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            Ingredient ingredient2 =
                new Ingredient("BBBBBBBBBB", "bubbly water", "Carbonated water is water " +
                "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            Ingredient ingredient3 =
                new Ingredient("CCCCCCCCC", "bubbly water", "Carbonated water is water " +
                "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            Ingredient ingredient4 =
                new Ingredient("AAAABBBB", "bubbly water", "Carbonated water is water " +
                "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            await fm.AddIngredientAsync(ingredient);
            await fm.AddIngredientAsync(ingredient2);
            await fm.AddIngredientAsync(ingredient3);
            await fm.AddIngredientAsync(ingredient4);
            List<Ingredient> testList = await fm.GetIngredientBySearchAsync("C", 0, 2);
            Assert.True((await fm.GetIngredientBySearchAsync("A", 0, 2)).Count == 2);
            Assert.True((await fm.GetIngredientBySearchAsync("A", 1, 2)).Count == 1);
            Assert.True(testList[0] == (ingredient3));

        }
        [Fact]
        public async void GetNIngredientsAsyncSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);

            Ingredient ingredient = new Ingredient("Carbinated", "bubbly water", "Carbonated water is water "
                + "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            Ingredient ingredient2 =
                new Ingredient("Carbinated2", "bubbly water", "Carbonated water is water " +
                "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            Ingredient ingredient3 =
                new Ingredient("Carbinated3", "bubbly water", "Carbonated water is water " +
                "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");
            Ingredient ingredient4 =
                new Ingredient("Carbinated4", "bubbly water", "Carbonated water is water " +
                "containing dissolved carbon dioxide gas, either artificially injected under " +
                "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            await fm.AddIngredientAsync(ingredient);
            await fm.AddIngredientAsync(ingredient2);
            await fm.AddIngredientAsync(ingredient3);
            await fm.AddIngredientAsync(ingredient4);

            Assert.True((await fm.GetNIngredientsAsync(0, 2)).Count == 2);
            Assert.True((await fm.GetNIngredientsAsync(1, 2)).Count == 2);
            Assert.True((await fm.GetNIngredientsAsync(1, 3)).Count == 3);
        }
        [Fact]
        public async void GetNutrientListForUserDisplaySuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = _random.Next();
            string barcode = _random.Next().ToString();
            FoodItem foodItem = new(barcode, "Monster Energy Drink", "CokaCola Co", "r/");
            Nutrient nutrient = new Nutrient("SomethingHealthy");
            nutrient.NutrientID = _random.Next();
            LabelIngredient ingredientIdentifier = new LabelIngredient(barcode, ingredient.IngredientID);
            LabelNutrient nutrientIdentifier = new LabelNutrient(barcode, nutrient.NutrientID, 0.14f);

            List<(Nutrient, float)> nutritionList = new();
            nutritionList.Add((nutrient, 0.1f));

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            Assert.True(await fm.AddNewProductAsync(foodItem, nutritionLabel, ingredientList));
            List<(Nutrient, float)> nutrientTestList = await fm.GetNutrientListForUserDisplayAsync(barcode);


            Assert.True(nutrientTestList.Count == nutritionList.Count);

        }

    }
}