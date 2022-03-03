using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class1;
using Console_Runner.Food;
using Food_Class_Library;

namespace Console_Runner.DAL
{
    public class EFFoodItemGateway : IFoodItemGateway
    {
        private readonly Context _efContext;

        public EFFoodItemGateway(Context dbContext)
        {
            _efContext = dbContext;
        }
        public bool addFoodItem(string barcode, string productName, string companyName, NutritionLabel nutritionLabel, List<Vitamins> vitaminsList, List<Ingredient> ingredientList)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public FoodItem retrieveScannedFoodItem(string barcode)
        {
            return _efContext.FoodItems.Find(barcode);
        }
    }
}
