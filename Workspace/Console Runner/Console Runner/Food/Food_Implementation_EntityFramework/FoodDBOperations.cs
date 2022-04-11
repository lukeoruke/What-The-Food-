using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public class FoodDBOperations
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IFoodGateway _foodItemAccess;
        private readonly LogService _logService;
        public FoodDBOperations(IFoodGateway foodItemAccess, LogService logService)
        {
            _foodItemAccess = foodItemAccess;
            _logService = logService;
        }

        //TODO: how to get user id???
        public async Task<bool> AddFoodItemAsync(FoodItem foodItem)
        {
            try
            {
                await _foodItemAccess.AddFoodItemAsync(foodItem);
                _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.Data, DateTime.Now,
                        $"Added food item \"{foodItem.ProductName}\"");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel)
        {
            var toReturn = await _foodItemAccess.AddNutritionLabelAsync(nutritionLabel);
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.Data, DateTime.Now,
                    $"Added nutrition label for food item {nutritionLabel.Barcode}");
            return toReturn;
        }

        public async Task<bool> AddNutrientAsync(Nutrient nutrient)
        {
            var toReturn = await _foodItemAccess.AddNutrientAsync(nutrient);
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.Data, DateTime.Now,
                    $"Added nutrient \"{nutrient.Name}\"");
            return toReturn;
        }

        public async Task<bool> AddNewProductAsync(FoodItem foodItem, NutritionLabel nutritionLabel, List<Ingredient> ingredientList)
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
                _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.Data, DateTime.Now,
                        $"Added product \"{foodItem.ProductName}\"");
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<NutritionLabel> GetNutritionLabelAsync(string barcode)
        {
            var label = await _foodItemAccess.RetrieveNutritionLabelAsync(barcode);
            if (label == null)
            {
                throw new Exception("No label exists for the provided barcode");
            }
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.Data, DateTime.Now,
                    $"Retrieved nutrition label for food item {barcode}");
            return label;
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient)
        {
            var toReturn = await _foodItemAccess.AddIngredientAsync(ingredient);
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.Data, DateTime.Now,
                    $"Added ingredient \"{ingredient.IngredientName}\"");
            return toReturn;
        }

        public async Task<List<Ingredient>> GetIngredientsListAsync(string barcode)
        {
            List<Ingredient> ingList = new List<Ingredient>();
            ingList = await _foodItemAccess.RetrieveIngredientListAsync(barcode);
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.Data, DateTime.Now,
                    $"Retrieved ingredients for item {barcode}");
            return ingList;
        }

        public async Task<FoodItem> GetScannedItemAsync(string barcode)
        {
            FoodItem? foodItem = await _foodItemAccess.RetrieveScannedFoodItemAsync(barcode);
            if(foodItem == null)
            {
                throw (new Exception("No such product exists in the DB"));
            }
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.Data, DateTime.Now,
                    $"Retrieved food item \"{foodItem.ProductName}\"");
            return foodItem;
        }
    }
}
