namespace Console_Runner.FoodService
{
    public class MemFoodItem : IFoodItem
    {
        List<FoodItem> _foodsList = new();
        List<NutritionLabel> _nutritionLabelsList = new();
        List<LabelIngredient> _ingredientIdentifiersList = new();
        List<Ingredient> _ingredientsList = new();




        //TODO REWORK FUNCTION ENTIRELY. CURRENT SETUP DOESNT ALLOW FOR UPDATED DB SETUP
        public async Task<bool> AddFoodItem(FoodItem foodItem)
        {
            try
            {
                _foodsList.Add(foodItem);
                return await true;
            }catch (Exception ex)
            {
                return false;
            }
            
            
        }

        public async bool AddIngredientAsync(Ingredient ingredient)
        {
            try
            {
                _ingredientsList.Add(ingredient);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async bool RemoveIngredientAsync(Ingredient ingredient)
        {
            try
            {
                _ingredientsList.Remove(ingredient);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async List<Ingredient> RetrieveIngredientListAsync(string barcode)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            for (int i = 0; i < _ingredientIdentifiersList.Count; i++)
            {
                if (_ingredientIdentifiersList[i].Barcode == barcode)
                {
                    for (int j = 0; j < _ingredientsList.Count; j++)
                    {
                        if (_ingredientsList[j].IngredientID == _ingredientIdentifiersList[i].IngredientID)
                        {
                            ingredients.Add(_ingredientsList[j]);
                        }
                    }
                }
            }
            return ingredients;
        }

        public async NutritionLabel? RetrieveNutritionLabelAsync(FoodItem food)
        {
            for (int i = 0; i < _nutritionLabelsList.Count; i++)
            {
                if (_nutritionLabelsList[i].Barcode == food.Barcode)
                {
                    return _nutritionLabelsList[i];
                }
            }
            return null;
        }

        public async FoodItem RetrieveScannedFoodItemAsync(string barcode)
        {
            for (int i = 0; i < _foodsList.Count; i++)
            {
                if (_foodsList[i].Barcode == barcode)
                {
                    return _foodsList[i];
                }
            }
            return null;
        }
    }
}
