using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{

    public class Label_FoodItem
    {
        //both of these together make a composit key 
        public string labelID { get; set; }
        public string barcode { get; set; }
        public Label_FoodItem()
        {
        }
        Label_FoodItem(string labelID, string barcode)
        {
            this.labelID = labelID; 
            this.barcode = barcode;
        }
    }
}
