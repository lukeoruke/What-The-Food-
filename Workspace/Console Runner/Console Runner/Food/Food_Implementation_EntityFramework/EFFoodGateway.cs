using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public class EFFoodGateway : IFoodGateway
    {
        private readonly ContextFoodDB _efContext;
        private readonly LogService _logService;

        public EFFoodGateway(LogService logService)
        {
            _efContext = new ContextFoodDB();
            _logService = logService;
        }

        //TODO: figure out how to get user id for log
        //TODO: need to catch possible exceptions from EF and log them individually
        public async Task<bool> AddLabelIngredientAsync(LabelIngredient labelIngredient)
        {
            try
            {
                if (!_efContext.LabelIngredients.Contains(labelIngredient))
                {
                    await _efContext.LabelIngredients.AddAsync(labelIngredient);
                    _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now, 
                            $"Created label-ingredient connection between barcode {labelIngredient.Barcode} and ingredient {labelIngredient.IngredientID}");
                    return true;
                }
                else
                {
                    _ = _logService.WriteLogAsync("placeholder", LogLevel.Debug, Category.DataStore, DateTime.Now, 
                            $"Label-ingredient connection between barcode {labelIngredient.Barcode} and ingredient {labelIngredient.IngredientID} already exists");
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> AddLabelNutrientAsync(LabelNutrient labelNutrient)
        {
            try
            {
                if (!_efContext.LabelNutrients.Contains(labelNutrient))
                {
                    await _efContext.LabelNutrients.AddAsync(labelNutrient);
                    _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Created label-nutrient connection between barcode {labelNutrient.Barcode} and nutrient {labelNutrient.NutrientID}");
                    return true;
                }
                else
                {
                    _ = _logService.WriteLogAsync("placeholder", LogLevel.Debug, Category.DataStore, DateTime.Now,
                            $"Label-nutrient connection between barcode {labelNutrient.Barcode} and nutrient {labelNutrient.NutrientID} already exists");
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddFoodItemAsync(FoodItem foodItem)
        {
            try
            {
                await _efContext.FoodItem.AddAsync(foodItem);
                await _efContext.SaveChangesAsync();
                _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created food item \"{foodItem.ProductName}\"");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel)
        {
            try
            {
                await _efContext.NutritionLabel.AddAsync(nutritionLabel);
                await _efContext.SaveChangesAsync();
                _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created nutrition label for food item {nutritionLabel.Barcode}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddNutrientAsync(Nutrient nutrient)
        {
            try
            {
                await _efContext.Nutrient.AddAsync(nutrient);
                await _efContext.SaveChangesAsync();
                _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created nutrient {nutrient.Name}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient)
        {
            try
            {
                await _efContext.Ingredients.AddAsync(ingredient);
                await _efContext.SaveChangesAsync();
                _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created ingredient {ingredient.IngredientName}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveIngredient(Ingredient ingredient)
        {
            try
            {
                _efContext.Ingredients.Remove(ingredient);
                _efContext.SaveChanges();
                _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Removed ingredient {ingredient.IngredientName}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Ingredient>> RetrieveIngredientListAsync(string barcode)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            var ListOfIngredients = _efContext.LabelIngredients.Where(r => r.Barcode == barcode);
            foreach(LabelIngredient ings in ListOfIngredients)
            {
                ingredients.Add(await _efContext.Ingredients.FindAsync(ings.IngredientID));
                _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Retrieved ingredient with ID {ings.IngredientID}");
            }
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved list of ingredients for nutrition label {barcode}");
            return ingredients;
        }

        public async Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode)
        {
            var toReturn = await _efContext.NutritionLabel.FindAsync(barcode);
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved nutrition label {barcode}");
            return toReturn;
        }

        public async Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode)
        {
            FoodItem? food = await _efContext.FoodItem.FindAsync(barcode);
            _ = _logService.WriteLogAsync("placeholder", LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved food item {food?.ProductName ?? "undefined"}");
            return food;
        }
    }
}
