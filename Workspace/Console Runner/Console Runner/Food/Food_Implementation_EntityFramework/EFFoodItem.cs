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
        public bool AddFoodItem(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList)
        {
            return false;
        }

        public bool AddIngredient(Ingredient ingredient)
        {
            try
            {
                _efContext.Ingredients.Add(ingredient);
                _efContext.SaveChanges();
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

        public List<Ingredient> RetrieveIngredientList(string barcode)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (var Ingredient in _efContext.LabelIngredients)
            {
                if (Ingredient.Barcode == barcode)
                {
                    ingredients.Add(_efContext.Ingredients.Find(barcode));
                }
            }
            return ingredients;
        }

        public NutritionLabel? RetrieveNutritionLabel(FoodItem food)
        {
            return _efContext.NutritionLabel.Find(food.Barcode);
        }

        public async Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode)
        {
            FoodItem? food = await _efContext.FoodItem.FindAsync(barcode);
            return food;
        }
    }
}
