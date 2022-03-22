﻿using Console_Runner.FoodService;
using System;
using Xunit;

namespace Test.FM
{
    public class FoodItemsShould
    {
        private Random _random = new Random();
        private readonly IFoodGateway _foodGateway = new MemFoodGateway();
        [Fact]
        public void AddFoodSuccess()
        {

            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = _random.Next();
            string barcode = _random.Next().ToString();
            FoodItem foodItem = new(barcode, "Monster Energy Drink", "CokaCola Co", "r/");
            LabelIngredient labelIdentifier = new LabelIngredient(barcode, ingredient.IngredientID);
            Nutrient nutrient = new Nutrient(foodItem.Barcode, "SomethingHealthy");
            List<Nutrient> nutritionList = new();
            nutritionList.Add(nutrient);

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            Assert.True(fm.AddFoodItem(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList));
        }
        [Fact]
        public void GetScannedFoodItemSuccess()
        {

            IFoodItemGateway foodItemGateway = new MemFoodItemGateway();
            IFlagGateway flagGateway = new MemFlagGateway();
            IlogGateway logGateway = new MemLogGateway();
            FM fm = new FM(foodItemGateway, flagGateway, logGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = "8";

            FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co");
            LabelIdentifier labelIdentifier = new LabelIdentifier(foodItem.Barcode, ingredient.IngredientID);
            Nutrient nutrient = new Nutrient(foodItem.Barcode, "SomethingHealthy");
            List<Nutrient> nutritionList = new();
            nutritionList.Add(nutrient);

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();   
            ingredientList.Add(ingredient);
            fm.AddFoodItem(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList);
            FoodItem? returnedFoodItem = fm.GetScannedFoodItem(foodItem.Barcode);
            Assert.True(returnedFoodItem != null);
            Assert.True(returnedFoodItem?.Equals(foodItem));

        }

        [Fact]
        public void GetNutritionLabelSuccess()
        {
            IFoodItemGateway foodItemGateway = new MemFoodItemGateway();
            IFlagGateway flagGateway = new MemFlagGateway();
            IlogGateway logGateway = new MemLogGateway();
            FM fm = new FM(foodItemGateway, flagGateway, logGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = "18";

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
        public void GetIngredientList()
        {
            IFoodItemGateway foodItemGateway = new MemFoodItemGateway();
            IFlagGateway flagGateway = new MemFlagGateway();
            IlogGateway logGateway = new MemLogGateway();
            FM fm = new FM(foodItemGateway, flagGateway, logGateway);

            Ingredient ingredient =
               new Ingredient("Carbinated Water", "bubbly water", "Carbonated water is water " +
               "containing dissolved carbon dioxide gas, either artificially injected under " +
               "pressure or occurring due to natural geological processes. Carbonation causes small bubbles to form, giving the water an effervescent quality.");

            ingredient.IngredientID = "1832";

            FoodItem foodItem = new("70847-841411116", "Monster Energy Drink", "CokaCola Co");
            LabelIdentifier labelIdentifier = new LabelIdentifier(foodItem.Barcode, ingredient.IngredientID);
            Nutrient nutrient = new Nutrient(foodItem.Barcode, "SomethingHealthy");
            List<Nutrient> nutritionList = new();
            nutritionList.Add(nutrient);

            NutritionLabel nutritionLabel = new NutritionLabel(270, 1, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, nutritionList, foodItem.Barcode);

            List<Ingredient> ingredientList = new List<Ingredient>();
            ingredientList.Add(ingredient);
            fm.AddFoodItem(foodItem, nutritionLabel, new List<Nutrient>(), ingredientList);
            List<Ingredient> ingredientListRetrieved = fm.GetIngredientList(foodItem.Barcode);
            Assert.True(ingredientListRetrieved.Count > 0);
            Assert.True(ingredientListRetrieved[0].Equals(ingredient));
        }


    }
}