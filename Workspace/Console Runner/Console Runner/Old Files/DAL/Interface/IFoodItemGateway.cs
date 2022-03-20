using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;
using Food_Class_Library;

namespace Console_Runner.DAL
{
    public interface IFoodItemGateway
    {
        public FoodItem? RetrieveScannedFoodItem(string barcode);
        public bool AddFoodItem(FoodItem foodItem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns>List containing all ingredeints in the food with the corosponding barcode</returns>
        public List<Ingredient> RetrieveIngredientList(string barcode);
        public bool AddIngredient(Ingredient ingredient);
        public bool RemoveIngredient(Ingredient ingredient);
        public NutritionLabel? RetrieveNutritionLabel(FoodItem food);
    }
}
