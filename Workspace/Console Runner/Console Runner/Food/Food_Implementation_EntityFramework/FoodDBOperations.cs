using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public class FoodDBOperations
    {
        private readonly IFoodGateway _foodItemAccess;
        public FoodDBOperations(IFoodGateway foodItemAccess)
        {
            _foodItemAccess = foodItemAccess;
        }

        public async Task<List<Ingredient>> GetIngredientBySearchAsync(string search, int skip, int take)
        {
            return await _foodItemAccess.GetIngredientBySearchAsync(search, skip, take);
        }

        public async Task<List<Ingredient>> GetNIngredients(int skip, int take)
        {
           return await _foodItemAccess.RetrieveNIngredientsAsync(skip, take);
        }

        public async Task<bool> AddFoodItemAsync(FoodItem foodItem)
        {
            try
            {
                await _foodItemAccess.AddFoodItemAsync(foodItem);
                // UserID not being null implies logService is not null
                if(logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Data, DateTime.Now,
                            $"Added food item \"{foodItem.ProductName}\"");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<List<(Nutrient, float)>> GetNutrientListForUserDisplay(string barcode)
        {
            
            List<LabelNutrient> temp =  await _foodItemAccess.RetrieveLabelNutrientByBarcodeAsync(barcode);
            List<(Nutrient, float)> nutrients = await _foodItemAccess.RetrieveNutrientListByIDAsync(temp);
            return nutrients;

        }

    public async Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel)
        {
            var toReturn = await _foodItemAccess.AddNutritionLabelAsync(nutritionLabel);
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Data, DateTime.Now,
                        $"Added nutrition label for food item {nutritionLabel.Barcode}");
            }
            return toReturn;
        }

        public async Task<bool> AddNutrientAsync(Nutrient nutrient, LogService? logService = null)
        {
            var toReturn = await _foodItemAccess.AddNutrientAsync(nutrient);
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Data, DateTime.Now,
                        $"Added nutrient \"{nutrient.Name}\"");
            }
            return toReturn;
        }

        public async Task<bool> AddNewProductAsync(FoodItem foodItem, NutritionLabel nutritionLabel, List<Ingredient> ingredientList, LogService? logService = null)
        {
            try
            {
                await _foodItemAccess.AddFoodItemAsync(foodItem);
                nutritionLabel.Barcode = foodItem.Barcode;
                await _foodItemAccess.AddNutritionLabelAsync(nutritionLabel);
                List<(Nutrient, float)>  vitaminsList = nutritionLabel.GetNutrientList();
                foreach (Ingredient ing in ingredientList)
                {
                    //will only add ing to DB if it doesnt already exist within the db
                    //If vitamin doesnt exist it will add it, creating an ID associated with that ingredient in the process
                    await _foodItemAccess.AddIngredientAsync(ing);
                    await _foodItemAccess.AddLabelIngredientAsync(new LabelIngredient(foodItem.Barcode, ing.IngredientID));
                }
                foreach ((Nutrient, float) vitamin in vitaminsList)
                {
                    //will only add vitamin to DB if it doesnt already exist within the db
                    //If vitamin doesnt exist it will add it, creating a NID associated with that vitamin in the process
                    await _foodItemAccess.AddNutrientAsync(vitamin.Item1);
                    await _foodItemAccess.AddLabelNutrientAsync(new LabelNutrient(foodItem.Barcode, vitamin.Item1.NutrientID, vitamin.Item2));
                }
                if(logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Data, DateTime.Now,
                            $"Added product \"{foodItem.ProductName}\"");
                }
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<NutritionLabel> GetNutritionLabelAsync(string barcode, LogService? logService = null)
        {
            var label = await _foodItemAccess.RetrieveNutritionLabelAsync(barcode);
            if (label == null)
            {
                throw new Exception("No label exists for the provided barcode");
            }
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved nutrition label for food item {barcode}");
            }
            return label;
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient, LogService? logService = null)
        {
            var toReturn = await _foodItemAccess.AddIngredientAsync(ingredient);
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Data, DateTime.Now,
                        $"Added ingredient \"{ingredient.IngredientName}\"");
            }
            return toReturn;
        }

        public async Task<List<Ingredient>> GetIngredientsListAsync(string barcode, LogService? logService = null)
        {
            List<Ingredient> ingList = new List<Ingredient>();
            ingList = await _foodItemAccess.RetrieveIngredientListAsync(barcode);
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved ingredients for item {barcode}");
            }
            return ingList;
        }

        public async Task<FoodItem> GetScannedItemAsync(string barcode, LogService? logService = null)
        {
            if(barcode == null)
            {
                throw (new Exception("provided barcode was null, this error is called from FoodDbOperations.GetScannedItemAsync() method "));
            }
            FoodItem?foodItem = await _foodItemAccess.RetrieveScannedFoodItemAsync(barcode);
            if(foodItem == null)
            {
                throw (new Exception("No such product exists in the DB"));
            }
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved food item \"{foodItem.ProductName}\"");
            }
            return foodItem;
        }
    }
}
