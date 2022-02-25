using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{

    public class FoodLabel
    {
        //both of these together make a composit key 
        public string labelID { get; set; }
        public string barcode { get; set; }
        public FoodLabel()
        {
        }
        FoodLabel(string labelID, string barcode)
        {
            this.labelID = labelID; 
            this.barcode = barcode;
        }
    }
}
