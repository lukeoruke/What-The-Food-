using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;

namespace Console_Runner.DAL
{
    public interface IFoodItemGateway
    {
        /// <summary>
        /// Gets a FoodItem from the database with barcode if it exists.
        /// </summary>
        /// <param name="barcode">The barcode of the FoodItem to search for.</param>
        /// <returns>The FoodItem with barcode if it exists, otherwise null.</returns>
        public FoodItem? RetrieveScannedFoodItem(string barcode);
        /// <summary>
        /// Adds a FoodItem to the database with the given arguments.
        /// </summary>
        /// <param name="barcode">The barcode of the FoodItem.</param>
        /// <param name="productName">Name of the FoodItem.</param>
        /// <param name="companyName">Name of the company that produces the FoodItem.</param>
        /// <param name="nutritionLabel">The nutrition label on the FoodItem.</param>
        /// <param name="vitaminsList">The vitamins list on the FoodItem.</param>
        /// <param name="ingredientList">The ingredients list of the FoodItem.</param>
        /// <returns>True if the FoodItem was created and added to the database, otherwise false.</returns>
        public bool AddFoodItem(string barcode, string productName, string companyName,
            NutritionLabel nutritionLabel, List<Vitamins> vitaminsList, List<Ingredient> ingredientList);
        /// <summary>
        /// Retrieves the List of Ingredients for a given FoodItem barcode.
        /// </summary>
        /// <param name="barcode">The barcode of the FoodItem to get the indredient list of.</param>
        /// <returns>A List of Ingredients belonging to the FoodItem with barcode if it exists, otherwise an empty list.</returns>
        public List<Ingredient> RetrieveIngredientList(string barcode);
        /// <summary>
        /// Adds an Ingredient to the database.
        /// </summary>
        /// <param name="ingredient">The Ingredient to add to the database.</param>
        /// <returns>True if the Ingredient was added to the database, otherwise false.</returns>
        public bool AddIngredient(Ingredient ingredient);
        /// <summary>
        /// Removes the Ingredient from the database.
        /// </summary>
        /// <param name="ingredient">The Ingredient to remove from the database.</param>
        /// <returns>True if the Ingredient was removed from the database, otherwise false.</returns>
        public bool RemoveIngredient(Ingredient ingredient);
        /// <summary>
        /// Retrieves the nutrition label for a given FoodItem from the database.
        /// </summary>
        /// <param name="food">The FoodItem whose NutritionLabel to retrieve.</param>
        /// <returns>The NutritionLabel associated with food if it is on the database, otherwise null.</returns>
        public NutritionLabel? RetrieveNutritionLabel(FoodItem food);
    }
}
