using Console_Runner.DAL;
using Food_Class_Library;
using LogAndArchive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class FM
    {
        private const string UM_CATEGORY = "Data Store";
        private ILogger logger;
        private IDataAccess dal;
        
        public FM(IDataAccess DAL, ILogger logging)
        {
           this.dal = DAL;
            this.logger = logging;
            logger = new Logging();
        }

        public bool addFlagToAccount(string email, string flag)
        {
            FoodFlag foodFlag = new(email, flag);
            return dal.addFlag(foodFlag);
      
        }

        public bool removeFoodFlag(string email, string IngredientID)
        {
            return dal.removeFoodFlag(email, IngredientID);
        }

        public List<FoodFlag> getAllAccountFlags(string email)
        {
            return dal.getAllAccountFlags(email);
        }
        
        //gets and returns the union of all ingredients in both the item corosponding to the given barcode and items that exist within the users flags.
        public List<Ingredient> productFlagCheck(string email, string barcode)
        {
            List<Ingredient> flaggedIngredientsInProduct = new();
            List<FoodFlag> userFlags = dal.getAllAccountFlags(email);
            List<Ingredient> ingredientList = dal.retrieveIngredientList(barcode);
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

        public List<Ingredient> foodContainsFlaggedItem(string barcode, string email)
        {
            FoodItem food = retrieveScannedFoodItem(barcode);
            List<Ingredient> flaggedIngredients = retrieveIngredientList(food.barcode);
            for (int i = 0; i < flaggedIngredients.Count; i++)
            {
                if (dal.accountHasFlag(email, flaggedIngredients[i].ingredientID))
                {
                    flaggedIngredients.Add(flaggedIngredients[i]);
                }
            }
            return flaggedIngredients;
        }

        public FoodItem retrieveScannedFoodItem(string barcode)
        {
            return dal.retrieveScannedFoodItem(barcode);
        }

        public NutritionLabel retrieveNutrtionLabel(FoodItem food)
        {
            return dal.retrieveNutrtionLabel(food);
        }

        public List<Ingredient> retrieveIngredientList(string labelID)
        {
            return dal.retrieveIngredientList(labelID);
        }

    }
}
