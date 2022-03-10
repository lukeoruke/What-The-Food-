using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;
using Food_Class_Library;

namespace Console_Runner.DAL
{
    public class EFFoodItemGateway : IFoodItemGateway
    {
        private readonly Context _efContext;

        public EFFoodItemGateway()
        {
            _efContext = new Context();
        }
        public bool AddFoodItem(string barcode, string productName, string companyName, NutritionLabel nutritionLabel, List<Vitamins> vitaminsList, List<Ingredient> ingredientList)
        {
            try
            {
                nutritionLabel.barcode = barcode;
                //Creates connection between barcode and list of food items connected to the corrosponding food item based on barcode
                for (int i = 0; i < ingredientList.Count; i++)
                {
                    LabelIdentifier label = new();
                    label.barcode = barcode;
                    label.ingredientID = ingredientList[i].ingredientID;
                    _efContext.IngredientIdentifier.Add(label);
                }
                for (int i = 0; i < vitaminsList.Count; i++)
                {
                    Vitamins vit = vitaminsList[i];
                    vit.barcode = barcode;
                    _efContext.Vitamins.Add(vit);
                }
                _efContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
            foreach (var Ingredient in _efContext.IngredientIdentifier)
            {
                if (Ingredient.barcode == barcode)
                {
                    ingredients.Add(_efContext.Ingredients.Find(barcode));
                }
            }
            return ingredients;
        }

        public NutritionLabel? RetrieveNutritionLabel(FoodItem food)
        {
            return _efContext.NutritionLabels.Find(food.barcode);
        }

        public FoodItem? RetrieveScannedFoodItem(string barcode)
        {
            return _efContext.FoodItems.Find(barcode);
        }
    }
}
