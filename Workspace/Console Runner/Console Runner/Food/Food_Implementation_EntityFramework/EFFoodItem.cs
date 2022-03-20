using Console_Runner.FoodService;

namespace Console_Runner.FoodService
{
    public class EFFoodItem : IFoodItem
    {
        private readonly ContextFoodDB _efContext;

        public EFFoodItem()
        {
            _efContext = new ContextFoodDB();
        }
        //TODO The add ingredient and nutrients dont account for the middle layer of label nutrient and label ingredient.
        public async Task<bool> AddNewProductAsync(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList)
        {
            try
            {

                foreach(Ingredient ing in ingredientList)
                {
                    await AddIngredientAsync(ing);
                }
                foreach(Nutrient nutrient in vitaminsList)
                {
                    await AddNutrientAsync(nutrient);
                }
                await AddNutritionLabelAsync(nutritionLabel);
                await AddFoodItem(foodItem);
            }catch (Exception)
            {
                throw new Exception("METHOD AddNewProductAsync encountered an un handled error");
            }
            return false;

        }

        public async Task<bool> AddFoodItem(FoodItem foodItem)
        {
            try
            {
                await _efContext.FoodItem.AddAsync(foodItem);
                await _efContext.SaveChangesAsync();
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
            }
            return ingredients;
        }

        public async Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode)
        {
            return await _efContext.NutritionLabel.FindAsync(barcode);
        }

        public async Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode)
        {
            FoodItem? food = await _efContext.FoodItem.FindAsync(barcode);
            return food;
        }
    }
}
