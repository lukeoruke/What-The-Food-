using Console_Runner.FoodService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodDBOperations
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IFoodFunctions _foodItemAccess;
        public FoodDBOperations(IFoodFunctions foodItemAccess)
        {
            this._foodItemAccess = foodItemAccess;
        }

        public async Task<bool> AddNewProductAsync(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList)
        {
            return await _foodItemAccess.AddNewProductAsync(foodItem, nutritionLabel, vitaminsList, ingredientList);
        }

        public async Task<NutritionLabel> GetNutritionLabelAsync(string barcode)
        {
            var label = await _foodItemAccess.RetrieveNutritionLabelAsync(barcode);
            if (label == null)
            {
                throw new Exception("No label exists for the provided barcode");
            }
            return label;
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient)
        {
            return await _foodItemAccess.AddIngredientAsync(ingredient);
        }

        public async Task<List<Ingredient>> GetIngredientsListAsync(string barcode)
        {
            List<Ingredient> ingList = new List<Ingredient>();
            ingList = await _foodItemAccess.RetrieveIngredientListAsync(barcode);
            return ingList;
        }

        public async Task<FoodItem> getScannedItemAsync(string barcode)
        {
            FoodItem?foodItem = await _foodItemAccess.RetrieveScannedFoodItemAsync(barcode);
            if(foodItem == null)
            {
                throw (new Exception("No such product exists in the DB"));
            }
            return foodItem;
        }
    }
}
