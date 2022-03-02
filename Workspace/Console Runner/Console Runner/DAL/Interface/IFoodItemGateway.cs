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
        public FoodItem retrieveScannedFoodItem(string barcode);
        public bool addFoodItem(string barcode, string productName, string companyName,
            NutritionLabel nutritionLabel, List<Vitamins> vitaminsList, List<Ingredient> ingredientList);
    }
}
