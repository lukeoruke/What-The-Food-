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
        [Fact]
        public async void AddNewProductSuccess()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

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

            List<Nutrient> nutritionList = new();
            nutritionList.Add(nutrient);

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            //Assert.True(await fm.AddNewProductAsync(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList));
        }
        [Fact]
        public async void GetScannedFoodItemSuccess()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            await fm.AddIngredientAsync(ingredient);
            string barcode = "lao109341";
            FoodItem foodItem = new(barcode, "Monster Energy Drink", "CokaCola Co", "r/");
            LabelIngredient labelIdentifier = new LabelIngredient(foodItem.Barcode, ingredient.IngredientID);
            Nutrient nutrient = new Nutrient("SomethingHealthy");
            List<Nutrient> nutritionList = new();
            nutritionList.Add(nutrient);

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            fm.AddFoodItemAsync(foodItem);
            FoodItem? returnedFoodItem = fm.GetScannedFoodItemAsync(foodItem.Barcode);
            Assert.True(returnedFoodItem != null);
            Assert.True(returnedFoodItem?.Equals(foodItem));

        }

        [Fact]
        public void GetNutritionLabelSuccess()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = _random.Next(1000)

            FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co");
            LabelIdentifier labelIdentifier = new LabelIdentifier(foodItem.Barcode, ingredient.IngredientID);
            Nutrient nutrient = new Nutrient(foodItem.Barcode, "SomethingHealthy");
            List<Nutrient> nutritionList = new();
            nutritionList.Add(nutrient);

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            fm.AddFoodItem(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList);

            NutritionLabel? returnedLabel = fm.GetNutrtionLabel(foodItem);
            Assert.True(returnedLabel?.Equals(nutritionLabel));
        }
        [Fact]
        public async void GetIngredientList()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = _random.Next(1000);

            FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co");
            LabelIngredient labelIdentifier = new LabelIngredient(foodItem.Barcode, ingredient.IngredientID);
            Nutrient nutrient = new Nutrient(foodItem.Barcode, "SomethingHealthy");
            List<Nutrient> nutritionList = new();
            nutritionList.Add(nutrient);

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            fm.AddFoodItem(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList);
            List<Ingredient> ingredientListRetrieved = await fm.GetIngredientsListAsync(foodItem.Barcode);
            Assert.True(ingredientListRetrieved.Count > 0);
            Assert.True(ingredientListRetrieved[0].Equals(ingredient));
        }


    }
}