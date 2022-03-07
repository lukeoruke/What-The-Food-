using Console_Runner.DAL;
using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class FM
    {
        private const string _UM_CATEGORY = "Data Store";
        private IFoodItemGateway _foodItemGateway;
        private IFlagGateway _flagGateway;
        private IlogGateway _logGateway;
        
        public FM(IFoodItemGateway _foodItemGateway, IFlagGateway _flagGateway,IlogGateway _logGateway)
        {
           this._foodItemGateway = _foodItemGateway;
            this._flagGateway = _flagGateway;
            this._logGateway = _logGateway;
        }

        public bool addFlagToAccount(string email, string flag)
        {
            FoodFlag foodFlag = new(email, flag);
            return _flagGateway.AddFlag(foodFlag);
      
        }

        public bool removeFoodFlag(string email, string IngredientID)
        {
            return _flagGateway.RemoveFoodFlag(email, IngredientID);
        }

        public List<FoodFlag> getAllAccountFlags(string email)
        {
            return _flagGateway.GetAllAccountFlags(email);
        }
        
        //gets and returns the union of all ingredients in both the item corosponding to the given barcode and items that exist within the users flags.
        public List<Ingredient> productFlagCheck(string email, string barcode)
        {
            List<Ingredient> flaggedIngredientsInProduct = new();
            List<FoodFlag> userFlags = _flagGateway.GetAllAccountFlags(email);
            List<Ingredient> ingredientList = _foodItemGateway.RetrieveIngredientList(barcode);
            for(int i = 0; i < ingredientList.Count; i++)
            {
                for(int j = 0; j < userFlags.Count; j++)
                {
                    if (ingredientList[i].ingredientID == userFlags[j].ingredientID)
                    {
                        flaggedIngredientsInProduct.Add(ingredientList[i]);
                    }
                }
                
            }
            return flaggedIngredientsInProduct;
        }

        public List<Ingredient> FoodContainsFlaggedItem(string barcode, string email)
        {
            FoodItem food = GetScannedFoodItem(barcode);
            List<Ingredient> flaggedIngredients = GetIngredientList(food.barcode);
            for (int i = 0; i < flaggedIngredients.Count; i++)
            {
                if (_flagGateway.AccountHasFlag(email, flaggedIngredients[i].ingredientID))
                {
                    flaggedIngredients.Add(flaggedIngredients[i]);
                }
            }
            return flaggedIngredients;
        }

        public FoodItem? GetScannedFoodItem(string barcode)
        {
            return _foodItemGateway.RetrieveScannedFoodItem(barcode);
        }

        public NutritionLabel? GetNutrtionLabel(FoodItem food)
        {
            return _foodItemGateway.RetrieveNutritionLabel(food);
        }

        public List<Ingredient> GetIngredientList(string labelID)
        {
            return _foodItemGateway.RetrieveIngredientList(labelID);
        }

        public bool AddIngredient(Ingredient ingredient)
        {
            try
            {
                _foodItemGateway.AddIngredient(ingredient);
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
        }

    }
}
