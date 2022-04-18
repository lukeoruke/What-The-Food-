using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public Ingredient GetIngredient(int id, LogService? logService = null)
        {
            Ingredient toReturn = _efContext.Ingredients.Where(x => x.IngredientID == id).ToList().ElementAt(0);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved ingredient {id}");
            }
            return toReturn;
        }
        public async Task<List<Ingredient>> GetIngredientBySearchAsync(string search, int skip, int take, LogService? logService = null)
        {
            List<Ingredient> results = await _efContext.Ingredients.Where(x => x.IngredientName.Contains(search))
                .OrderBy(x => x.IngredientName).Skip(skip).Take(take).ToListAsync();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved list of {take} ingredients by name \"{search}\"");
            }
            return results;
        }

        public async Task<List<Ingredient>>RetrieveNIngredientsAsync(int skip, int take, LogService? logService = null)
        {
            List<Ingredient> results = await _efContext.Ingredients.OrderBy(x => x.IngredientName)
                .Skip(skip).Take(take).ToListAsync();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved list of {take} ingredients");
            }
            return results;
        }

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
            var ListOfIngredients = await _efContext.LabelIngredients.Where(r => r.Barcode == barcode).ToListAsync();
            for(int i = 0; i < ListOfIngredients.Count(); i++)
            {
                ingredients.Add(await _efContext.Ingredients.FindAsync(ListOfIngredients[i].IngredientID));
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved list of ingredients for label {barcode}");
            }
            return ingredients;
        }

        public async Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode, LogService? logService = null)
        {
            NutritionLabel? nutritionLabel = await _efContext.NutritionLabel.FindAsync(barcode);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved nutrition label by barcode {barcode}");
            }
            return nutritionLabel;
        }

        public async Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode, LogService? logService = null)
        {
            FoodItem? food = await _efContext.FoodItem.FindAsync(barcode);
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved food item by barcode {barcode}");
            }
            return food;
        }

        public async Task<List<LabelNutrient>> RetrieveLabelNutrientByBarcodeAsync(string barcode, LogService? logService = null)
        {
            List<LabelNutrient> toReturn = _efContext.LabelNutrients.Where(r => r.Barcode == barcode).ToList();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved list of label-nutrient connections for label {barcode}");
            }
            return toReturn;
        }

        public async Task<List<(Nutrient, float)>> RetrieveNutrientListByIDAsync(List<LabelNutrient> list, LogService? logService = null)
        {
            List<(Nutrient?, float)> nutrientList = new();
            for (int i = 0; i < list.Count; i++)
            {
                 nutrientList.Add((await _efContext.Nutrient.FindAsync(list[i].NutrientID), list[i].NutrientPercentage));
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved list of nutrients by ID");
            }
            return nutrientList;
        }
    }
}
