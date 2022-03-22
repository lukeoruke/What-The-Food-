

namespace Console_Runner.FoodService
{
    public class MemFoodGateway : IFoodGateway
    {
        List<FoodItem> _foodsList = new();
        List<NutritionLabel> _nutritionLabelsList = new();
        List<LabelIngredient> _ingredientIdentifiersList = new();
        List<Ingredient> _ingredientsList = new();
        List<Nutrient> _vitaminList = new();

        public async Task<bool> AddFoodItem(FoodItem foodItem)
        {
            try
            {
                _foodsList.Add(foodItem);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add food item in-memory: "+ e);
                return false;
            }
        }
        public async Task<List<Ingredient>> RetrieveIngredientListAsync(string barcode)
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
                        }
                    }
                }
            }
            return listOfIngredientsInProduct;
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient)
        {
            try
            {
                _ingredientsList.Add(ingredient);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add ingredients in-memory: " + ex);
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
                Console.WriteLine("Failed to remove ingredients in-memory: " + ex);
                return false;
            }

        }
        public async Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode)
        {
            foreach(FoodItem fooditem in _foodsList)
            {
                if(fooditem.Barcode == barcode)
                {
                    return fooditem;
                }
            }
            return null;
        }
        public async Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode)
        {
            foreach(NutritionLabel label in _nutritionLabelsList)
            {
                if(label.Barcode == barcode)
                {
                    return label;
                }
            }
            return null;
        }
        public async Task<bool> AddNewProductAsync(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList)
        {
            try
            {
                foreach(Ingredient ing in ingredientList)
                {
                    _ingredientsList.Add(ing);
                }
                foreach(Nutrient nutrient in vitaminsList)
                {
                    _vitaminList.Add(nutrient);
                }
                _nutritionLabelsList.Add(nutritionLabel);
                _foodsList.Add(foodItem);

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to add new product in-memory: " + e);
                return false;
            }
        }
        public async Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel)
        {
            try
            {
                _nutritionLabelsList.Add(nutritionLabel);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add nutrition label in-memory: " + e);
                return false;
            }
        }
        public async Task<bool> AddNutrientAsync(Nutrient nutrient)
        {
            try
            {
                _vitaminList.Add(nutrient);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to add nutrient in-memory: " + e);
                return false;
            }

        }

    }
}
