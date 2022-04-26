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

        /// <summary>
        /// Gets an ingredient based on an ingredient ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logService"></param>
        /// <returns>the ingredient with the specified ingredientID</returns>
        public async Task<Ingredient> GetIngredient(int id, LogService? logService = null)
        {
            Ingredient ing = _foodItemAccess.GetIngredient(id);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved ingredient {id}");
            }
            return ing;
        }
        /// <summary>
        /// Gets ingredients that match the search criteria
        /// </summary>
        /// <param name="search"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="logService"></param>
        /// <returns>a list of all ingredients that meet the search criteria and fall within the skip and take params</returns>
        public async Task<List<Ingredient>> GetIngredientBySearchAsync(string search, int skip, int take, LogService? logService = null)
        {
            List<Ingredient> ingList = await _foodItemAccess.GetIngredientBySearchAsync(search, skip, take);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.Data, DateTime.Now,
                        $"Retrieved {take} ingredients after {skip} with name containing \"{search}\"");
            }
            return ingList;
        }
        /// <summary>
        /// Gets N ingredients at a time
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="logService"></param>
        /// <returns>Returns N ingredients, N being take</returns>
        public async Task<List<Ingredient>> GetNIngredientsAsync(int skip, int take, LogService? logService = null)
        {
            List<Ingredient> ingList = await _foodItemAccess.RetrieveNIngredientsAsync(skip, take);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved {take} ingredients after {skip}");
            }
            return ingList;
        }
        /// <summary>
        /// Adds an object of type FoodItem to the DB
        /// </summary>
        /// <param name="foodItem"></param>
        /// <returns>True if successful, otherwise false</returns>
        public async Task<bool> AddFoodItemAsync(FoodItem foodItem, LogService? logService = null)
        {
            try
            {
                await _foodItemAccess.AddFoodItemAsync(foodItem);
                // UserID not being null implies logService is not null
                if(logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                            $"Added food item \"{foodItem.ProductName}\"");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        /// <summary>
        /// Gets the label nutrient list associated with a product from its barcode
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="logService"></param>
        /// <returns>a list of all labelNutrients associated with a specific product</returns>
        public async Task<List<(Nutrient, float)>> GetNutrientListForUserDisplayAsync(string barcode, LogService? logService = null)
        {
            List<LabelNutrient> temp =  await _foodItemAccess.RetrieveLabelNutrientByBarcodeAsync(barcode);
            List<(Nutrient, float)> nutrients = await _foodItemAccess.RetrieveNutrientListByIDAsync(temp);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved nutrient list for label {barcode}");
            }
            return nutrients;
        }
        /// <summary>
        /// Adds a Nutrition Label to the DB
        /// </summary>
        /// <param name="nutritionLabel"></param>
        /// <returns>True if successful, otherwise false</returns>
        public async Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel, LogService? logService = null)
        {
            var toReturn = await _foodItemAccess.AddNutritionLabelAsync(nutritionLabel);
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Added nutrition label for food item {nutritionLabel.Barcode}");
            }
            return toReturn;
        }
        /// <summary>
        /// Adds a Nutrient to the DB
        /// </summary>
        /// <param name="nutrient"></param>
        /// <returns><returns>True if successful, otherwise false</returns></returns>
        public async Task<bool> AddNutrientAsync(Nutrient nutrient, LogService? logService = null)
        {
            var toReturn = await _foodItemAccess.AddNutrientAsync(nutrient);
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Added nutrient \"{nutrient.Name}\"");
            }
            return toReturn;
        }
        /// <summary>
        /// Adds an object of type FoodItem to the DB
        /// </summary>
        /// <param name="foodItem"></param>
        /// <returns>True if successful, otherwise false</returns>
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
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                            $"Added product \"{foodItem.ProductName}\"");
                }
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        /// <summary>
        /// Gets the nutrition label associated with a provided barcode
        /// </summary>
        /// <param name="barcode">The barcode being searched</param>
        /// <returns>The nutrition label associated with a provided barcode</returns>
        public async Task<NutritionLabel> GetNutritionLabelAsync(string barcode, LogService? logService = null)
        {
            var label = await _foodItemAccess.RetrieveNutritionLabelAsync(barcode);
            if (label == null)
            {
                throw new Exception("No label exists for the provided barcode");
            }
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved nutrition label for food item {barcode}");
            }
            return label;
        }
        /// <summary>
        /// Adds an ingredient to the DB
        /// </summary>
        /// <param name="ingredient">The Ingredient being added</param>
        /// <returns>True if the opperation was successful, false otherwise.</returns>
        public async Task<bool> AddIngredientAsync(Ingredient ingredient, LogService? logService = null)
        {
            var toReturn = await _foodItemAccess.AddIngredientAsync(ingredient);
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Added ingredient \"{ingredient.IngredientName}\"");
            }
            return toReturn;
        }
        /// <summary></summary>
        /// <param name="barcode"></param>
        /// <returns>List containing all ingredeints in the food with the corosponding barcode</returns>
        public async Task<List<Ingredient>> GetIngredientsListAsync(string barcode, LogService? logService = null)
        {
            List<Ingredient> ingList = new List<Ingredient>();
            ingList = await _foodItemAccess.RetrieveIngredientListAsync(barcode);
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved ingredients for item {barcode}");
            }
            return ingList;
        }
        /// <summary>
        /// Gets a food object corosponding to the provided barcode
        /// </summary>
        /// <param name="barcode">the barcode being searched</param>
        /// <returns>a food item corosponding to the provided barcode</returns>
        public async Task<FoodItem> GetScannedItemAsync(string barcode, LogService? logService = null)
        {
            if(barcode == null)
            {
                throw (new Exception("provided barcode was null, this error is called from FoodDbOperations.GetScannedItemAsync() method "));
            }
            FoodItem?foodItem = await _foodItemAccess.RetrieveScannedFoodItemAsync(barcode);
            if(foodItem == null)
            {
                //throw (new Exception("No such product exists in the DB"));
                return null;
            }
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.Data, DateTime.Now,
                        $"Retrieved food item \"{foodItem.ProductName}\"");
            }
            return foodItem;
        }
    }
}
