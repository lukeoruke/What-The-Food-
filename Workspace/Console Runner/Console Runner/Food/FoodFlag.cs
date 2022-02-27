using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class FoodFlag
    {
        public string accountEmail;
        public string ingredientID;
        public FoodFlag()
        {
        }
        public FoodFlag (string accountEmail, string ingredientID)
        {
            this.accountEmail = accountEmail;
            this.ingredientID = ingredientID;
        }
    }
}
