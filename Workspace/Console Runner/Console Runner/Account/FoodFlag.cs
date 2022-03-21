using Console_Runner;
using Console_Runner.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Console_Runner.AMRModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Console_Runner.AccountService
{
    public class FoodFlag
    {
        //Utilizing User ID from Account as a Foreign Key
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public string IngredientID { get; set; }
        

        //Constructor
        public FoodFlag(int userID, string ingredientID) {
            this.UserID = userID;
            this.IngredientID = ingredientID;
        }
       
    }
}
