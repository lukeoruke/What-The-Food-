namespace Console_Runner.FoodService
{
    public class MemFoodItem : IFoodItem
    {
        List<FoodItem> _foodsList = new();
        List<NutritionLabel> _nutritionLabelsList = new();
        List<LabelIngredient> _ingredientIdentifiersList = new();
        List<Ingredient> _ingredientsList = new();




        //TODO REWORK FUNCTION ENTIRELY. CURRENT SETUP DOESNT ALLOW FOR UPDATED DB SETUP
        public bool AddFoodItem(FoodItem fooditem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList)
        {
            try
            {
                string barcode = fooditem.Barcode;
                nutritionLabel.Barcode = barcode;
                _nutritionLabelsList.Add(nutritionLabel);
                _foodsList.Add(fooditem);

                //Creates connection between barcode and list of food items connected to the corrosponding food item based on barcode
                for (int i = 0; i < ingredientList.Count; i++)
                {
                    LabelIngredient label = new();
                    label.Barcode = barcode;
                    label.IngredientID = ingredientList[i].IngredientID;
                    _ingredientIdentifiersList.Add(label);
                    _ingredientsList.Add(ingredientList[i]);
                }
                for (int i = 0; i < vitaminsList.Count; i++)
                {
                    Nutrient vit = vitaminsList[i];
                    vit.Barcode = barcode;
                    vitaminsList.Add(vit);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddIngredient(Ingredient ingredient)
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

        public bool RemoveIngredient(Ingredient ingredient)
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

        public List<Ingredient> RetrieveIngredientList(string barcode)
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

        public NutritionLabel? RetrieveNutritionLabel(FoodItem food)
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

        public FoodItem RetrieveScannedFoodItemAsync(string barcode)
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
