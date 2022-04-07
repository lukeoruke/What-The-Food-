using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodUpdate
    {
        public FoodItem FoodItem { get; set; }
        public int FoodItemId { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Message { get; set; }

        public FoodUpdate()
        {

        }

        public FoodUpdate(FoodItem foodItem, DateTime updateTime, string message)
        {
            FoodItem = foodItem;
            UpdateTime = updateTime;
            Message = message;
        }
    }
}
