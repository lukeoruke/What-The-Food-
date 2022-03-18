using Console_Runner;
using Console_Runner.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Console_Runner.AMRModel;

namespace Console_Runner.AccountDB
{
    public class FoodFlag
    {
        //Utilizing User ID from Account as a Foreign Key
        public string UserID { get; set; }
        public string IngredientID { get; set; }
        

        //Constructor
        public FoodFlag(string userID, string ingredientID) {
            this.UserID = userID;
            this.IngredientID = ingredientID;
        }
       
    }
}
