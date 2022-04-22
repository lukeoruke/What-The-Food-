using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodUpdate
    {
        public int Id { get; set; }
        public FoodItem FoodItem { get; set; }
        public string FoodItemBarcode { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Message { get; set; }

        public FoodUpdate()
        {

        }

        public FoodUpdate(FoodItem foodItem, DateTime updateTime, string message)
        {
            FoodItem = foodItem;
            FoodItemBarcode = foodItem.Barcode;
            UpdateTime = updateTime;
            Message = message;
        }
    }
}
