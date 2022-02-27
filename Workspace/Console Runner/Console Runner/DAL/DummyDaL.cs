using Console_Runner.Food;
using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner.DAL
{
    public class DummyDaL : IDataAccess
    {

        List<Account> accountsList = new();
        List<user_permissions> permissionsList = new();
        List<FoodItem> foodsList = new();
        List<FoodFlag> flagsList = new();
        List<Ingredient> ingredientsList = new();
        List<NutritionLabel> nutritionLabelsList = new();
        List<LabelIdentifyer> ingredientIdentifyersList = new();

        public bool accountExists(string email)
        {
            for(int i = 0; i < accountsList.Count; i++)
            {
                if (email == accountsList[i].Email){
                    return true;
                }
            }
            return false;
        }

        public bool addHistoryItem()
        {
            throw new NotImplementedException();
        }

        public bool addAccount(Account acc)
        {
            accountsList.Add(acc);

            return true;
        }

   

        public bool addPermission(string email, string permission)
        {
            user_permissions newPermission = new user_permissions(email, permission,this);
            permissionsList.Add(newPermission);
            return true;
        }

        public int AdminCount()
        {
            int count = 0;
            for(int i = 0; i < accountsList.Count(); i++)
            {
                if (hasPermission(accountsList[i].Email, "createAdmin") && accountsList[i].isActive)
                {
                    count++;
                }
            }
            return count;
        }

  

        public Account getAccount(string email)
        {
            for(int i = 0;i < accountsList.Count;i++)
            {
                if(accountsList[i].Email == email)
                {
                    return accountsList[i];
                }
            }
            return null;
        }

     

        public List<user_permissions> getAllUserPermissions(string email)
        {
            List<user_permissions> usersPerms = new List<user_permissions>();
            for(int i = 0; i < permissionsList.Count;i++)
            {
                if(permissionsList[i].email == email)
                {
                    usersPerms.Add(permissionsList[i]);
                }
            }
            return usersPerms;
        }

        public bool hasPermission(string email, string permission)
        {
            for(int i = 0; i < permissionsList.Count ; i++)
            {
                if(permissionsList[i].email == email && permissionsList[i].permission == permission)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isAdmin(string email)
        {
            for (int i = 0; i < permissionsList.Count; i++)
            {
                if (permissionsList[i].email == email && permissionsList[i].permission == "createAdmin")
                {
                    return true;
                }
            }
            return false;
        }

        public bool removeAccount(Account acc)
        {
            for(int i = 0; i < accountsList.Count;i++)
            {
                if(accountsList[i].Email == acc.Email)
                {
                    accountsList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool removeAllUserPermissions(string email)
        {
            for(int i = 0; i <permissionsList.Count ; i++)
            {
                if (permissionsList[i].email == email)
                {
                    permissionsList.RemoveAt(i);
                    return true;
                }
            }
            return false ;
        }

    

        public bool removePermision(string email, string permission)
        {
            try
            {
                user_permissions perm = new user_permissions(email, permission,this);
                permissionsList.Remove(perm);
                return true;
            }catch (Exception e)
            {
                return false ;
            }
            
        }





        public bool updateAccount(Account acc)
        {
            for(int i = 0; i < accountsList.Count ; i++)
            {
                if(accountsList[i].Email == acc.Email)
                {
                    accountsList[i] = acc;
                    return true;
                }
            }
            return false;
        }

        /////////////////////////////////////////////////////////////////////FOOD FLAGS////////////////////////////////////////////////////////////////////////////////////////////
        public bool addFlag(FoodFlag flag)
        {
            if(flag != null && !flagsList.Contains(flag))
            {
                flagsList.Add(flag);
                return true;
            }
            return false;
        }

        public bool accountHasFlag(string email, string ingredientID)
        {
            FoodFlag flag = new FoodFlag(email, ingredientID);
            return flagsList.Contains(flag);
        }

        public bool removeFoodFlag(string email, string ingredientID)
        {
            if (accountHasFlag(email, ingredientID))
            {
                FoodFlag foodFlag = new(email, ingredientID);
                flagsList.Remove(foodFlag);
                return true;
            }
            return false;
        }

        public List<FoodFlag> getAllAccountFlags(string email)
        {
            List<FoodFlag> flagList = new List<FoodFlag>();
            foreach (var flag in flagsList)
            {
                if (flag.accountEmail == email)
                {
                    flagList.Add(flag);
                }
            }
            return flagList;
        }


        ///////////////////////////////////////////////////////////////////////////////////FOOD ITEMS//////////////////////////////////////////////////////////////////////////////////////////

        public FoodItem retrieveScannedFoodItem(string barcode)
        {
            for(int i = 0; i < foodsList.Count; i++)
            {
                if (foodsList[i].barcode == barcode)
                {
                    return foodsList[i];
                }
            }
            return null;
        }

        public NutritionLabel retrieveNutrtionLabel(FoodItem food)
        {
            for (int i = 0; i < nutritionLabelsList.Count; i++)
            {
                if (nutritionLabelsList[i].barcode == food.labelID)
                {
                    return nutritionLabelsList[i];
                }
            }
            return null;
        }

        public List<Ingredient> retrieveIngredientList(string labelID)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            for(int i = 0; i < ingredientIdentifyersList.Count; i++)
            {
                if(ingredientIdentifyersList[i].barcode == labelID)
                {
                    for(int j = 0; j < ingredientsList.Count; j++)
                    {
                        if(ingredientsList[j].ingredientID == ingredientIdentifyersList[i].ingredientID)
                        {
                            ingredients.Add(ingredientsList[j]);
                        }
                    }
                }
            }
            return ingredients;
        }
        public bool addFoodItem(string barcode, string productName, string companyName, NutritionLabel label, List<Ingredient> ingredients, List<Vitamins> vitamins)
        {
            FoodItem foodItem = new FoodItem(barcode, productName, companyName);
            return false;
        }


    }
}
