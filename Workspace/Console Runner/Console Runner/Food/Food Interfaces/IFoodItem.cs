using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public interface IFoodItem
    {
        public bool AddFoodItem(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns>List containing all ingredeints in the food with the corosponding barcode</returns>
        public List<Ingredient> RetrieveIngredientList(string barcode);
        public bool AddIngredient(Ingredient ingredient);
        public bool RemoveIngredient(Ingredient ingredient);
        public Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode);
    }
}
