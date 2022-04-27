using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodUpdate
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public FoodItem FoodItem { get; set; }
        [JsonIgnore]
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
            UpdateTime = updateTime.Date;
            Message = message;
        }
    }
}
