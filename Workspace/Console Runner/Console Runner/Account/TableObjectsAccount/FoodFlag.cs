
using System.ComponentModel.DataAnnotations.Schema;

namespace Console_Runner.AccountService
{
    public class FoodFlag
    {
        //Utilizing User ID from Account as a Foreign Key
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public int IngredientID { get; set; }
        

        //Constructor
        public FoodFlag(int userID, int ingredientID) {
            this.UserID = userID;
            this.IngredientID = ingredientID;
        }
        public FoodFlag()
        {

        }
       
    }
}
