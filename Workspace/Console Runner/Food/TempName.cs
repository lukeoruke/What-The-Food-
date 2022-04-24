using Console_Runner.FoodService;

namespace Food.Executables
{
    public class TempName
    {
        private IFoodGateway _foodGateway = new EFFoodGateway();
        private IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();
        FoodDBOperations fm;
        public TempName()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway, _foodUpdateGateway);
        }

        public FoodItem makeFoodItem(string barcode, string prodName, string compName, string pic)
        {
            FoodItem foodItem = new FoodItem(barcode, prodName, compName, pic);
            return foodItem;
        }

        public NutritionLabel makeNutritionLabel(int calories, int servings, double servingSize,
            int totalFat, int saturatedFat, int transFat, int cholestrol, int sodium,
            int totalCarbohydrate, int dietaryFiber, int totalSugars, int addedSugar,
            int protein, List<(Nutrient, float)> additionalNutrients, string barcode) {
            NutritionLabel label = new NutritionLabel(calories, servings, servingSize, totalFat, saturatedFat, transFat,
                cholestrol, sodium, totalCarbohydrate, dietaryFiber, totalSugars, addedSugar, protein, additionalNutrients, barcode);
            return label;
        }

        public List<Ingredient> makeIngredient(List<string> ingredientName, List<string> alternateName, List<string> ingredientDescription)
        {
            List<Ingredient> ingredientLabel = new List<Ingredient>();
            for(int i = 0; i < ingredientName.Count; i++)
            {
                Ingredient ingredient = new Ingredient(ingredientName[i], alternateName[i], ingredientDescription[i]);
                ingredientLabel.Add(ingredient);
            }

            return ingredientLabel;
        }

        public async void addFoodToDatabase(FoodItem f, NutritionLabel n, List<Ingredient> i)
        {
            await fm.AddNewProductAsync(f, n, i);
        }
    }
}
