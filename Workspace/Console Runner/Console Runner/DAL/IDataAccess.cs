using Console_Runner.Food;
using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.User_Management;

namespace Console_Runner.DAL
{
    public interface IDataAccess
    {
        public bool accountExists(string email);
        public Account getAccount(string email);
        public bool addAccount(Account acc);
        public bool removeAccount(Account acc);
        public bool updateAccount(Account acc);
        public bool hasPermission(string email, string permission);
        public bool addPermission(string email, string permission);
        public bool removePermision(string email, string permission);
        public List<Permission> getAllUserPermissions(string email);
        public bool removeAllUserPermissions(string email);
        public int AdminCount();
        public bool addHistoryItem();
        public bool isAdmin(string email);
        public bool accountHasFlag(string email, string flag);
        public bool removeFoodFlag(string email, string flag);
        public List<FoodFlag> getAllAccountFlags(string email);

        public FoodItem retrieveScannedFoodItem(string barcode);
        public List<Ingredient> retrieveIngredientList(string labelID);
        public bool addFoodItem(string barcode, string productName, string companyName, 
            NutritionLabel nutritionLabel, List<Vitamins> vitaminsList, List<Ingredient> ingredientList);
        public bool addFlag(FoodFlag flag);
        public NutritionLabel retrieveNutrtionLabel(FoodItem food);
        public bool addIngredient(Ingredient ingredient);
        public bool removeIngredient(Ingredient ingredient);
    }
}
