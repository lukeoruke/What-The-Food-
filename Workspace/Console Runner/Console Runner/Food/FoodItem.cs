using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
   public class FoodItem
    {
        public string barcode { get; set; }
        public string productName { get; set; }
        public string companyName { get; set; }
        //public string labelID { get; set; }
        public FoodItem()
        {
        }
        public FoodItem(string barcode, string productName, string companyName)
        {
            this.barcode = barcode;
            this.productName = productName;
            this.companyName = companyName;
        }
       

    }
}
