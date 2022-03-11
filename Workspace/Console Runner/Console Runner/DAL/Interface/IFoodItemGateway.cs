using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;

namespace Console_Runner.DAL
{
    public interface IFoodItemGateway
    {
        public FoodItem? RetrieveScannedFoodItem(string barcode);
        public bool AddFoodItem(FoodItem foodItem, NutritionLabel nutritionLabel, List<Vitamins> vitaminsList, List<Ingredient> ingredientList);
        public List<Ingredient> RetrieveIngredientList(string barcode);
        public bool AddIngredient(Ingredient ingredient);
        public bool RemoveIngredient(Ingredient ingredient);
        public NutritionLabel? RetrieveNutritionLabel(FoodItem food);
    }
}
