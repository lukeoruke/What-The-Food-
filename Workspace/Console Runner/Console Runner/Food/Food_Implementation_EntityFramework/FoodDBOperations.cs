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
        private readonly IFoodItem _foodItemAccess;
        public FoodDBOperations(IFoodItem foodItemAccess)
        {
            this._foodItemAccess = foodItemAccess;
        }

        public bool AddFoodItem(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList)
        {
            return _foodItemAccess.AddFoodItem(foodItem, nutritionLabel, vitaminsList, ingredientList);
        }

        public async Task<bool> AddFoodItemAsync(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList)
        {
            return await Task.FromResult(_foodItemAccess.AddFoodItem(foodItem, nutritionLabel, vitaminsList, ingredientList));
        }

        public FoodItem getScannedItem(string barcode)
        {
            FoodItem? foodItem = _foodItemAccess.RetrieveScannedFoodItemAsync(barcode);
            if(foodItem == null)
            {
                throw (new Exception("No such product exists in the DB"));
            }
            return foodItem;
        }
    }
}
