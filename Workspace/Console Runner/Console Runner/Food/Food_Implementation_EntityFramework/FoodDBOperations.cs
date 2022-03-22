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
        private readonly IFoodGateway _foodItemAccess;
        public FoodDBOperations(IFoodGateway foodItemAccess)
        {
            this._foodItemAccess = foodItemAccess;
        }

        public async Task<bool> AddNewProductAsync(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList,
            LabelNutrient labelNutrient, List<Ingredient> ingredientList, LabelIngredient labelIngredient)
        {
            return await _foodItemAccess.AddNewProductAsync(foodItem, nutritionLabel, vitaminsList, labelNutrient, ingredientList, labelIngredient);
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
