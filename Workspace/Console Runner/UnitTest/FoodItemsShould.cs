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

            List<(Nutrient, float)> nutritionList = new();
            nutritionList.Add((nutrient, 0.1f));

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            //Assert.True(await fm.AddNewProductAsync(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList));
        }
        [Fact]
        public async void GetScannedFoodItemSuccess()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

            /*            Ingredient ingredient =
                           new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
                           "containing dissolved carbon dioxide gas, either artificially injected under " +
                           "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

                        await fm.AddIngredientAsync(ingredient);
            

                       LabelIngredient labelIdentifier = new LabelIngredient(foodItem.Barcode, ingredient.IngredientID);
                        Nutrient nutrient = new Nutrient("SomethingHealthy");
                        List<Nutrient> nutritionList = new();
                        nutritionList.Add(nutrient);

                        NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

                        List<Ingredient> ingredientList = new List<Ingredient>();
                        ingredientList.Add(ingredient);*/
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
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

            /*            Ingredient ingredient =
                           new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
                           "containing dissolved carbon dioxide gas, either artificially injected under " +
                           "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

                        ingredient.IngredientID = _random.Next(1000);

                        FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co","/rDir");
                        LabelIngredient labelIdentifier = new LabelIngredient(foodItem.Barcode, ingredient.IngredientID);
                        Nutrient nutrient = new Nutrient(foodItem.Barcode, "SomethingHealthy", 0.01f);
                        List<Nutrient> nutritionList = new();
                        nutritionList.Add(nutrient);
                        List<Ingredient> ingredientList = new List<Ingredient>();
                        ingredientList.Add(ingredient);
                        fm.AddFoodItemAsync(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList);*/



            FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co", "/rDir");
            Nutrient nutrient = new Nutrient("SomethingHealthy");
            await fm.AddNutrientAsync(nutrient);
            List<(Nutrient,float )> nutritionList = new();
            nutritionList.Add((nutrient, 0.23f));
            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);
            await fm.AddNutritionLabelAsync(nutritionLabel);


            NutritionLabel? returnedLabel = await fm.GetNutritionLabelAsync(foodItem.Barcode);
            Assert.True(returnedLabel?.Equals(nutritionLabel));
        }
        [Fact]
        public async void GetIngredientList()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

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

            ingredient.IngredientID = _random.Next(1000);

            FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co", "r/asda");
            LabelIngredient labelIdentifier = new LabelIngredient(foodItem.Barcode, ingredient.IngredientID);
            Nutrient nutrient = new Nutrient("SomethingHealthy");
            fm.AddNutrientAsync(nutrient);
            List<(Nutrient,float)> nutritionList = new();
            nutritionList.Add((nutrient,0.5f));

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


    }
}