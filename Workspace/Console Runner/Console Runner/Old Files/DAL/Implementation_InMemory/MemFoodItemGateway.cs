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
        List<LabelIdentifier> _ingredientIdentifiersList = new();
        List<Ingredient> _ingredientsList = new();
        public bool AddFoodItem(FoodItem fooditem, NutritionLabel nutritionLabel, List<Nutrient> vitaminsList, List<Ingredient> ingredientList)
        {
            try
            {
                string barcode = fooditem.Barcode;
                nutritionLabel.Barcode = barcode;
                _nutritionLabelsList.Add(nutritionLabel);
                _foodsList.Add(fooditem);

                //Creates connection between barcode and list of food items connected to the corrosponding food item based on barcode
                for (int i = 0; i < ingredientList.Count; i++)
                {
                    LabelIdentifier label = new();
                    label.Barcode = barcode;
                    label.IngredientID = ingredientList[i].IngredientID;
                    _ingredientIdentifiersList.Add(label);
                    _ingredientsList.Add(ingredientList[i]);
                }
                for (int i = 0; i < vitaminsList.Count; i++)
                {
                    Nutrient vit = vitaminsList[i];
                    vit.Barcode = barcode;
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
                if (_ingredientIdentifiersList[i].Barcode == barcode)
                {
                    for (int j = 0; j < _ingredientsList.Count; j++)
                    {
                        if (_ingredientsList[j].IngredientID == _ingredientIdentifiersList[i].IngredientID)
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
                if (_nutritionLabelsList[i].Barcode == food.Barcode)
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
                if (_foodsList[i].Barcode == barcode)
                {
                    return _foodsList[i];
                }
            }
            return null;
        }
    }
}
