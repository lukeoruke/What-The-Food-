

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



        public async Task<bool> AddLabelIngredientAsync(LabelIngredient labelIngredient)
        {
            try
            {
                if (!_ingredientIdentifiersList.Contains(labelIngredient) && labelIngredient is not null)
                {
                    _ingredientIdentifiersList.Add(labelIngredient);
                    return true;
                }
                else
                {
                    return false;
                }
                
            }catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> AddLabelNutrientAsync(LabelNutrient labelNutrient)
        {
            try
            {
                if (!_nutrientIdentifiersList.Contains(labelNutrient) && labelNutrient is not null)
                {
                    _nutrientIdentifiersList.Add(labelNutrient);
                    return true;
                }
                else
                {
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
                if(! _ingredientsList.Contains(ingredient))
                {
                    Random _random = new Random();
                    ingredient.IngredientID = _random.Next(1, 10000000);
                    _ingredientsList.Add(ingredient);
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
                if(!_vitaminList.Contains(nutrient))
                {
                    Random random = new Random();
                    nutrient.NutrientID = random.Next(1,10000000);
                    _vitaminList.Add(nutrient);
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
        public async Task<List<(Nutrient, float)>> RetrieveNutrientListByIDAsync(List<LabelNutrient> list)
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
            return nutrientList;
        }
        //TODO IMPLEMENT THIS
        public async Task<List<LabelNutrient>> RetrieveLabelNutrientByBarcodeAsync(string barcode)
        {
            List<LabelNutrient> list = new();
            for(int i = 0; i < _nutrientIdentifiersList.Count; i++)
            {
                if(_nutrientIdentifiersList[i].Barcode == barcode)
                {
                    list.Add(_nutrientIdentifiersList[i]);
                }
            }
            return list;
        }
    }
}
