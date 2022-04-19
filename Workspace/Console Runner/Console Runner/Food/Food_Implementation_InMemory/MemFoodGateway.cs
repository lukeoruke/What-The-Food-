using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public class MemFoodGateway : IFoodGateway
    {
        List<FoodItem> _foodsList = new();
        List<NutritionLabel> _nutritionLabelsList = new();
        List<LabelIngredient> _ingredientIdentifiersList = new();
        List<Ingredient> _ingredientsList = new();
        List<Nutrient> _vitaminList = new();
        List<LabelNutrient> _nutrientIdentifiersList = new();
        List<Nutrient> _nutrientsList = new();

        public async Task<bool> AddLabelIngredientAsync(LabelIngredient labelIngredient, LogService? logService = null)
        {
            try
            {
                if (!_ingredientIdentifiersList.Contains(labelIngredient) && labelIngredient is not null)
                {
                    _ingredientIdentifiersList.Add(labelIngredient);
                    if (logService?.UserID != null)
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
                
            }catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> AddLabelNutrientAsync(LabelNutrient labelNutrient, LogService? logService = null)
        {
            try
            {
                if (!_nutrientIdentifiersList.Contains(labelNutrient) && labelNutrient is not null)
                {
                    _nutrientIdentifiersList.Add(labelNutrient);
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
                _foodsList.Add(foodItem);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Created food item \"{foodItem.ProductName}\"");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add food item in-memory: "+ e);
                return false;
            }
        }
        public async Task<List<Ingredient>> RetrieveIngredientListAsync(string barcode, LogService? logService = null)
        {
            List<Ingredient> listOfIngredientsInProduct = new List<Ingredient>();
            foreach(LabelIngredient label in _ingredientIdentifiersList)
            {
                if(label.Barcode == barcode)
                {
                    foreach(Ingredient ingredient in _ingredientsList)
                    {
                        if(label.IngredientID == ingredient.IngredientID)
                        {
                            listOfIngredientsInProduct.Add(ingredient);
                            if (logService?.UserID != null)
                            {
                                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                        $"Retrieved ingredient with ID {ingredient}");
                            }
                        }
                    }
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Retrieved list of ingredients for nutrition label {barcode}");
            }
            return listOfIngredientsInProduct;
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient, LogService? logService = null)
        {
            try
            {
                if(! _ingredientsList.Contains(ingredient))
                {
                    Random _random = new Random();
                    ingredient.IngredientID = _random.Next(1, 10000000);
                    _ingredientsList.Add(ingredient);
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                $"Created ingredient {ingredient.IngredientName}");
                    }
                    return true;
                }
                return false;//ingredient already existed
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add ingredients in-memory: " + ex);
                return false;
            }

        }
        public bool RemoveIngredient(Ingredient ingredient, LogService? logService = null)
        {
            try
            {
                _ingredientsList.Remove(ingredient);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Removed ingredient {ingredient.IngredientName}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to remove ingredients in-memory: " + ex);
                return false;
            }

        }
        public async Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode, LogService? logService = null)
        {
            foreach(FoodItem fooditem in _foodsList)
            {
                if(fooditem.Barcode == barcode)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                $"Retrieved food item {fooditem?.ProductName ?? "undefined"}");
                    }
                    return fooditem;
                }
            }
            return null;
        }
        public async Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode, LogService? logService = null)
        {
            foreach(NutritionLabel label in _nutritionLabelsList)
            {
                if(label.Barcode == barcode)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                $"Retrieved nutrition label {barcode}");
                    }
                    return label;
                }
            }
            return null;
        }
        
        public async Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel, LogService? logService = null)
        {
            try
            {
                _nutritionLabelsList.Add(nutritionLabel);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Created nutrition label for food item {nutritionLabel.Barcode}");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add nutrition label in-memory: " + e);
                return false;
            }
        }
        public async Task<bool> AddNutrientAsync(Nutrient nutrient, LogService? logService = null)
        {
            try
            {
                if(!_vitaminList.Contains(nutrient))
                {
                    Random random = new Random();
                    nutrient.NutrientID = random.Next(1,10000000);
                    _vitaminList.Add(nutrient);
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                $"Created nutrient {nutrient.Name}");
                    }
                    return true;
                }
                return false; //already existed in DB
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to add nutrient in-memory: " + e);
                return false;
            }

        }
        //TODO IMPLEMENT THIS
        public async Task<List<(Nutrient, float)>> RetrieveNutrientListByIDAsync(List<LabelNutrient> list, LogService? logService = null)
        {

            List<(Nutrient?, float)> nutrientList = new();
            for (int i = 0; i < list.Count; i++)
            {
                for(int j = 0; j < _nutrientsList.Count; j++)
                {
                    if(list[i].NutrientID == _nutrientsList[j].NutrientID)
                    {
                        nutrientList.Add((_nutrientsList[j],list[i].NutrientPercentage));
                    }
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved list of nutrients by ID");
            }
            return nutrientList;
        }
        //TODO IMPLEMENT THIS
        public async Task<List<LabelNutrient>> RetrieveLabelNutrientByBarcodeAsync(string barcode, LogService? logService = null)
        {
            List<LabelNutrient> list = new();
            for(int i = 0; i < _nutrientIdentifiersList.Count; i++)
            {
                if(_nutrientIdentifiersList[i].Barcode == barcode)
                {
                    list.Add(_nutrientIdentifiersList[i]);
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                        $"Retrieved list of label-nutrient connections for label {barcode}");
            }
            return list;
        }


        //TODO IMPLEMENT THIS!!!
        public async Task<List<Ingredient>> RetrieveAllIngredientsAsync(LogService? logService = null)
        {
            return _ingredientsList;
        }

        public async Task<List<Ingredient>> GetIngredientBySearchAsync(string search, int skip, int take, LogService? logService = null)
        {
            List<Ingredient> ingredientsMatchingSearch = new List<Ingredient>();
            for(int i = 0; i < _ingredientsList.Count; i++)
            {
                if(_ingredientsList[i].IngredientName.Contains(search))
                {
                    ingredientsMatchingSearch.Add(_ingredientsList[i]);
                }
            }
            List<Ingredient> subList = new();
            for(int i = skip; i < ingredientsMatchingSearch.Count; i++)
            {
                if(take == 0)
                {
                    return subList;
                }
                subList.Add(ingredientsMatchingSearch[i]);
                take--;
            }
           return subList;
        }

        public async Task<List<Ingredient>> RetrieveNIngredientsAsync(int skip, int take, LogService? logService = null)
        {
            List<Ingredient> ingsList = new List<Ingredient>();
            for (int i = skip; i < _ingredientsList.Count; i++)
            {
                if (take == 0)
                {
                    return ingsList;
                }
                ingsList.Add(_ingredientsList[i]);
                take--;
            }
            return ingsList;
        }

        public Ingredient GetIngredient(int id, LogService? logService = null)
        {
            return _ingredientsList.Where(r => r.IngredientID == id).ToList()[0];
        }
    }
}
