using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public interface IFoodGateway
    {
        public Task<bool> AddLabelNutrientAsync(LabelNutrient labelNutrient, LogService? logService = null);
        public Task<bool> AddLabelIngredientAsync(LabelIngredient labelIngredient, LogService? logService = null);

        /// <summary>
        /// Adds an object of type FoodItem to the DB
        /// </summary>
        /// <param name="foodItem"></param>
        /// <returns>True if successful, otherwise false</returns>
        public Task<bool> AddFoodItemAsync(FoodItem foodItem, LogService? logService = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns>List containing all ingredeints in the food with the corosponding barcode</returns>
        public Task<List<Ingredient>> RetrieveIngredientListAsync(string barcode, LogService? logService = null);
        /// <summary>
        /// Adds an ingredient to the DB
        /// </summary>
        /// <param name="ingredient">The Ingredient being added</param>
        /// <returns>True if the opperation was successful, false otherwise.</returns>
        public Task<bool> AddIngredientAsync(Ingredient ingredient, LogService? logService = null);
        /// <summary>
        /// Removes an Ingredient from the DB
        /// </summary>
        /// <param name="ingredient">The ingredient being removed</param>
        /// <returns>True if the opperation was successful, false otherwise.</returns>
        public bool RemoveIngredient(Ingredient ingredient, LogService? logService = null);
        /// <summary>
        /// Gets a food object corosponding to the provided barcode
        /// </summary>
        /// <param name="barcode">the barcode being searched</param>
        /// <returns>a food item corosponding to the provided barcode</returns>
        public Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode, LogService? logService = null);
        /// <summary>
        /// Gets the nutrition label associated with a provided barcode
        /// </summary>
        /// <param name="barcode">The barcode being searched</param>
        /// <returns>The nutrition label associated with a provided barcode</returns>
        public Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode, LogService? logService = null);

        /// <summary>
        /// Adds a Nutrition Label to the DB
        /// </summary>
        /// <param name="nutritionLabel"></param>
        /// <returns>True if successful, otherwise false</returns>
        public Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel, LogService? logService = null);
        /// <summary>
        /// Adds a Nutrient to the DB
        /// </summary>
        /// <param name="nutrient"></param>
        /// <returns><returns>True if successful, otherwise false</returns></returns>
        public Task<bool> AddNutrientAsync(Nutrient nutrient, LogService? logService = null);
    }
}
