﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public interface IFoodItem
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns>List containing all ingredeints in the food with the corosponding barcode</returns>
        public Task<List<Ingredient>> RetrieveIngredientListAsync(string barcode);
        /// <summary>
        /// Adds an ingredient to the DB
        /// </summary>
        /// <param name="ingredient">The Ingredient being added</param>
        /// <returns>True if the opperation was successful, false otherwise.</returns>
        public Task<bool> AddIngredientAsync(Ingredient ingredient);
        /// <summary>
        /// Removes an Ingredient from the DB
        /// </summary>
        /// <param name="ingredient">The ingredient being removed</param>
        /// <returns>True if the opperation was successful, false otherwise.</returns>
        public bool RemoveIngredient(Ingredient ingredient);
        /// <summary>
        /// Gets a food object corosponding to the provided barcode
        /// </summary>
        /// <param name="barcode">the barcode being searched</param>
        /// <returns>a food item corosponding to the provided barcode</returns>
        public Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode);
        /// <summary>
        /// Gets the nutrition label associated with a provided barcode
        /// </summary>
        /// <param name="barcode">The barcode being searched</param>
        /// <returns>The nutrition label associated with a provided barcode</returns>
        public Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode);




        public Task<bool> AddNewProductAsync(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList);
        public Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel);
        public Task<bool> AddNutrientAsync(Nutrient nutrient);
    }
}
