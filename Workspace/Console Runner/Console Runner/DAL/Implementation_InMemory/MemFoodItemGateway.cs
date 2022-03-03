using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;
using Food_Class_Library;

namespace Console_Runner.DAL
{
    public class MemFoodItemGateway : IFoodItemGateway
    {
        List<FoodItem> _foodsList = new();
        List<NutritionLabel> _nutritionLabelsList = new();
        List<LabelIdentifyer> _ingredientIdentifiersList = new();
        List<Ingredient> _ingredientsList = new();
        public bool AddFoodItem(string barcode, string productName, string companyName, NutritionLabel nutritionLabel, List<Vitamins> vitaminsList, List<Ingredient> ingredientList)
        {
            try
            {
                nutritionLabel.barcode = barcode;
                //Creates connection between barcode and list of food items connected to the corrosponding food item based on barcode
                for (int i = 0; i < ingredientList.Count; i++)
                {
                    LabelIdentifyer label = new();
                    label.barcode = barcode;
                    label.ingredientID = ingredientList[i].ingredientID;
                    _ingredientIdentifiersList.Add(label);
                }
                for (int i = 0; i < vitaminsList.Count; i++)
                {
                    Vitamins vit = vitaminsList[i];
                    vit.barcode = barcode;
                    vitaminsList.Add(vit);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddIngredient(Ingredient ingredient)
        {
            try
            {
                _ingredientsList.Add(ingredient);
                return true;
            }
            catch (Exception ex)
            {
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
                return false;
            }
        }

        public List<Ingredient> RetrieveIngredientList(string barcode)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            for (int i = 0; i < _ingredientIdentifiersList.Count; i++)
            {
                if (_ingredientIdentifiersList[i].barcode == barcode)
                {
                    for (int j = 0; j < _ingredientsList.Count; j++)
                    {
                        if (_ingredientsList[j].ingredientID == _ingredientIdentifiersList[i].ingredientID)
                        {
                            ingredients.Add(_ingredientsList[j]);
                        }
                    }
                }
            }
            return ingredients;
        }

        public NutritionLabel? RetrieveNutritionLabel(FoodItem food)
        {
            for (int i = 0; i < _nutritionLabelsList.Count; i++)
            {
                if (_nutritionLabelsList[i].barcode == food.barcode)
                {
                    return _nutritionLabelsList[i];
                }
            }
            return null;
        }

        public FoodItem RetrieveScannedFoodItem(string barcode)
        {
            for (int i = 0; i < _foodsList.Count; i++)
            {
                if (_foodsList[i].barcode == barcode)
                {
                    return _foodsList[i];
                }
            }
            return null;
        }
    }
}
