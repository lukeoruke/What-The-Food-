using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public class EFFoodGateway : IFoodGateway
    {
        private readonly ContextFoodDB _efContext;

        public EFFoodGateway()
        {
            _efContext = new ContextFoodDB();
        }

        //TODO: figure out how to get user id for log
        //TODO: need to catch possible exceptions from EF and log them individually
        public async Task<bool> AddLabelIngredientAsync(LabelIngredient labelIngredient, LogService? logService = null)
        {
            try
            {
                if (!_efContext.LabelIngredients.Contains(labelIngredient))
                {
                    await _efContext.LabelIngredients.AddAsync(labelIngredient);
                    if(logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                $"Created label-ingredient connection between barcode {labelIngredient.Barcode} and ingredient {labelIngredient.IngredientID}");
                    }
                    return true;
                }
                else
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                                $"Label-ingredient connection between barcode {labelIngredient.Barcode} and ingredient {labelIngredient.IngredientID} already exists");
                    }
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> AddLabelNutrientAsync(LabelNutrient labelNutrient, LogService? logService = null)
        {
            try
            {
                if (!_efContext.LabelNutrients.Contains(labelNutrient))
                {
                    await _efContext.LabelNutrients.AddAsync(labelNutrient);
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                $"Created label-nutrient connection between barcode {labelNutrient.Barcode} and nutrient {labelNutrient.NutrientID}");
                    }
                    return true;
                }
                else
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                                $"Label-nutrient connection between barcode {labelNutrient.Barcode} and nutrient {labelNutrient.NutrientID} already exists");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddFoodItemAsync(FoodItem foodItem, LogService? logService = null)
        {
            try
            {
                await _efContext.FoodItem.AddAsync(foodItem);
                await _efContext.SaveChangesAsync();
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Created food item \"{foodItem.ProductName}\"");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel, LogService? logService = null)
        {
            try
            {
                await _efContext.NutritionLabel.AddAsync(nutritionLabel);
                await _efContext.SaveChangesAsync();
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Created nutrition label for food item {nutritionLabel.Barcode}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddNutrientAsync(Nutrient nutrient, LogService? logService = null)
        {
            try
            {
                await _efContext.Nutrient.AddAsync(nutrient);
                await _efContext.SaveChangesAsync();
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Created nutrient {nutrient.Name}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient, LogService? logService = null)
        {
            try
            {
                await _efContext.Ingredients.AddAsync(ingredient);
                await _efContext.SaveChangesAsync();
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Created ingredient {ingredient.IngredientName}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveIngredient(Ingredient ingredient, LogService? logService = null)
        {
            try
            {
                _efContext.Ingredients.Remove(ingredient);
                _efContext.SaveChanges();
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Removed ingredient {ingredient.IngredientName}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Ingredient>> RetrieveIngredientListAsync(string barcode, LogService? logService = null)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            var ListOfIngredients = _efContext.LabelIngredients.Where(r => r.Barcode == barcode);
            foreach(LabelIngredient ings in ListOfIngredients)
            {
                ingredients.Add(await _efContext.Ingredients.FindAsync(ings.IngredientID));
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Retrieved ingredient with ID {ings.IngredientID}");
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Retrieved list of ingredients for nutrition label {barcode}");
            }
            return ingredients;
        }

        public async Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode, LogService? logService = null)
        {
            var toReturn = await _efContext.NutritionLabel.FindAsync(barcode);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Retrieved nutrition label {barcode}");
            }
            return toReturn;
        }

        public async Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode, LogService? logService = null)
        {
            FoodItem? food = await _efContext.FoodItem.FindAsync(barcode);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Retrieved food item {food?.ProductName ?? "undefined"}");
            }
            return food;
        }
    }
}
