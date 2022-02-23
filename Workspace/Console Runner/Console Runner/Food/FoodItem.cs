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
        [Key]
        public string barcode { get; set; }
        string productName { get; set; }
        string companyName { get; set; }
        public FoodItem()
        {
        }
        public FoodItem(string barcode, string productName, string companyName, string labelID)
        {
            this.barcode = barcode;
            this.productName = productName;
            this.companyName = companyName;
        }
       

    }
}
