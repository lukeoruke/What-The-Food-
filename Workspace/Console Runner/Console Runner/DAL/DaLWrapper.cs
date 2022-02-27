using Class1;
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
    public class DaLWrapper : IDataAccess
    {
        Context context = new();
        public DaLWrapper()
        {

        }

        /////////////////////////////////////////////////////////////Accounts////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Used to verifying if an account exists in the database with the provided email(primary key)
        /// </summary>
        /// <param name="email">account email, acts as the PK</param>
        /// <returns>true if account exists, false otherwise</returns>
        public bool accountExists(string email)
        {

            if (context.accounts.Find(email) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Used for retrieving an account object from the db
        /// </summary>
        /// <param name="email">Account email, acting as PK</param>
        /// <returns>account object with the provided email(PK) assuming it exists, will return null if the account does not exist</returns>
        public Account getAccount(string email)
        {
            try
            {
                Account acc = context.accounts.Find(email);
                if (acc != null)
                {
                    return acc;
                }
                else
                {
                    throw new Exception("account not found exception");
                }
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        /// <summary>
        /// adds an account object to the DB
        /// </summary>
        /// <param name="acc"> an account object that will be added to the DB</param>
        /// <returns>True if the operation was successful, false if it failed</returns>
        public bool addAccount(Account acc)
        {
            try
            {
                context.accounts.Add(acc);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        /// <summary>
        /// Removes an account from the DB
        /// </summary>
        /// <param name="acc">the acc object being removed from the db</param>
        /// <returns>true if operation was ssuccessfulm false otherwise</returns>
        public bool removeAccount(Account acc)
        {
            try
            {
                if (accountExists(acc.Email))
                {
                    context.Remove(acc);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Updates an account object in the DB. Modify the account object, then pass it into this method. The data in the DB will change to the modified version
        /// </summary>
        /// <param name="acc">The updated version of the acc</param>
        /// <returns>true if operation was successful, false otherwise</returns>
        public bool updateAccount(Account acc)
        {
            try
            {
                context.accounts.Update(acc);
                context.SaveChanges(true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        /////////////////////////////////////////////////////////////Permissions////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Checks if the specified user has a specified permission
        /// </summary>
        /// <param name="email">the specified users PK</param>
        /// <param name="permission">the permission we are checking for</param>
        /// <returns>true if the user has the specified permission false if they do not</returns>
        public bool hasPermission(string email, string permission)
        {
            return context.permissions.Find(email, permission) != null;
        }
        public bool addPermission(string email, string permission)
        {
            try
            {
                user_permissions newPermission = new user_permissions(email, permission, this);
                if (context.permissions.Find(email, permission) == null)
                {
                    context.permissions.Add(newPermission);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Removess a speciofied permission from the specified user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="permission"></param>
        /// <returns>true if operation was successful false otherwise</returns>
        public bool removePermision(string email, string permission)
        {
            try
            {
                user_permissions newPermission = new user_permissions(email, permission, this);
                if (hasPermission(email, permission))
                {
                    context.permissions.Remove(newPermission);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Gets a list of all permissions associated with a users account
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<user_permissions> getAllUserPermissions(string email)
        {
            List<user_permissions> alluserPermissions = new List<user_permissions>();
            foreach (var permissions in context.permissions)
            {
                if (permissions.email == email)
                {
                    alluserPermissions.Add(permissions);
                }
            }
            return alluserPermissions;
        }

        /// <summary>
        /// removes all permissions associated with an account(mainly used for cascade deletion)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool removeAllUserPermissions(string email)
        {
            foreach (var permissions in context.permissions)
            {
                if (permissions.email == email)
                {
                    context.permissions.Remove(permissions);
                }
            }
            return true;
        }

        //returns the number of admins in the database
        public int AdminCount()
        {
            int count = 0;
            using (var context = new Context())
            {
                foreach (var account in context.accounts)
                {
                    if (hasPermission(account.Email, "createAdmin") && account.isActive) count++;
                }
            }
            return count;
        }

        public bool isAdmin(string email)
        {
            using (var context = new Context())
            {
                foreach (var account in context.accounts)
                {
                    if (hasPermission(account.Email, "createAdmin") && account.isActive)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool addHistoryItem()
        {
            throw new NotImplementedException();
        }
        /////////////////////////////////////////////////////////////////////FOOD FLAGS////////////////////////////////////////////////////////////////////////////////////////////
       public bool addFlag(FoodFlag flag)
        {
            try
            {
                context.foodFlags.Add(flag);
                context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }

        public bool accountHasFlag(string email, string ingredientID)
        {
            FoodFlag foodFlag = new(email, ingredientID);
            return context.foodFlags.Find(foodFlag) != null;
        }

        public bool removeFoodFlag(string email, string ingredientID)
        {
            if (accountHasFlag(email, ingredientID))
            {
                FoodFlag foodFlag = new(email, ingredientID);
                context.foodFlags.Remove(foodFlag);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<FoodFlag> getAllAccountFlags(string email)
        {
            List <FoodFlag> flagList = new List<FoodFlag>();
            foreach (var flag in context.foodFlags)
            {
                if(flag.accountEmail == email)
                {
                    flagList.Add(flag);
                }
            }
            return flagList;
        }

 
        ///////////////////////////////////////////////////////////////////////////////////FOOD ITEMS//////////////////////////////////////////////////////////////////////////////////////////

        public FoodItem retrieveScannedFoodItem(string barcode)
        {
            return context.foodItems.Find(barcode);
        }

        public NutritionLabel retrieveNutrtionLabel(FoodItem food)
        {
            return context.nutritionLabels.Find(food.barcode);
        }

        public List<Ingredient> retrieveIngredientList(string barcode)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (var Ingredient in context.ingredientIdentifyer)
            {
                if(Ingredient.barcode == barcode)
                {
                    ingredients.Add(context.ingredients.Find(barcode));
                }
            }
            return ingredients;
        }
        public bool addFoodItem(string barcode, string productName, string companyName, NutritionLabel label, List<Ingredient> ingredients, List<Vitamins> vitamins)
        {
            FoodItem foodItem = new FoodItem(barcode, productName, companyName);
            return false;
        }
        public bool createNewProduct(string barcode, string productName, string companyName, NutritionLabel nutritionLabel, List<Vitamins> vitaminsList, List<Ingredient> ingredientList)
        {
            nutritionLabel.barcode = barcode;
            //Creates connection between barcode and list of food items connected to the corrosponding food item based on barcode
            for (int i = 0; i < ingredientList.Count; i++)
            {
                LabelIdentifyer label = new();
                label.barcode = barcode;
                label.ingredientID = ingredientList[i].ingredientID;
                context.ingredientIdentifyer.Add(label);
            }
            for(int i = 0; i < vitaminsList.Count; i++)
            {
                Vitamins vit = vitaminsList[i];
                vit.barcode = barcode;
                context.vitamins.Add(vit);
            }
            context.SaveChanges();
        }

    }
}
