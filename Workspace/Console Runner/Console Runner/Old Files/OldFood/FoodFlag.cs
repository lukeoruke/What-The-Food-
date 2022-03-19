using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class FoodFlag
    {
        public string AccountEmail { get; set; }
        public string IngredientID { get; set; }
        public FoodFlag()
        {
        }
        public FoodFlag (string accountEmail, string ingredientID)
        {
            this.AccountEmail = accountEmail;
            this.IngredientID = ingredientID;
        }
    }
}
