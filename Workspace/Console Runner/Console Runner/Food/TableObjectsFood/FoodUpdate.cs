using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public enum UpdateType
    {
        Recall,
        RecipeChange
    }
    public class FoodUpdate
    {
        public FoodItem FoodItem { get; set; }
        public int FoodItemId { get; set; }
        public DateTime UpdateTime { get; set; }
        public UpdateType UpdateType { get; set; }
        public string Message { get; set; }

        public FoodUpdate()
        {

        }

        public FoodUpdate(FoodItem foodItem, DateTime updateTime, UpdateType updateType, string message)
        {
            FoodItem = foodItem;
            UpdateTime = updateTime;
            UpdateType = updateType;
            Message = message;
        }
    }
}
