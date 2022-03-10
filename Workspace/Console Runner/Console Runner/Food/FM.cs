using Console_Runner.DAL;
using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Logging;

namespace Console_Runner.Food
{
    public class FM
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IFoodItemGateway _foodItemGateway;
        private readonly IFlagGateway _flagGateway;
        private readonly ILogger _logGateway;
        
        public FM(IFoodItemGateway foodItemGateway, IFlagGateway flagGateway, ILogger logGateway)
        {
            _foodItemGateway = foodItemGateway;
            _flagGateway = flagGateway;
            _logGateway = logGateway;
        }

        public bool AddFlagToAccount(string email, string flag)
        {
            FoodFlag foodFlag = new(email, flag);
            return _flagGateway.AddFlag(foodFlag);
        }

        public bool RemoveFoodFlag(string email, string IngredientID)
        {
            return _flagGateway.RemoveFoodFlag(email, IngredientID);
        }

        public List<FoodFlag> GetAllAccountFlags(string email)
        {
            return _flagGateway.GetAllAccountFlags(email);
        }
        
        //gets and returns the union of all ingredients in both the item corosponding to the given barcode and items that exist within the users flags.
        public List<Ingredient> CheckProductForFlags(string email, string barcode)
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
            FoodItem? food = GetScannedFoodItem(barcode);
            if (food == null) return new List<Ingredient>();
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
            }catch (Exception)
            {
                return false;
            }
            
        }

    }
}
