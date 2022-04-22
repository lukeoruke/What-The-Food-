using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodRecall : FoodUpdate
    {
        public List<string> Locations { get; set; }
        public List<int> LotNumbers { get; set; }
        public List<DateTime> ExpirationDates { get; set; }
        public string Reason { get; set; }

        public FoodRecall() : base()
        {
        }

        public FoodRecall(FoodItem foodItem, DateTime updateTime, string message,
            string reason, IEnumerable<string> locations, IEnumerable<int> lotNumbers, IEnumerable<DateTime> expirationDates) : base(foodItem, updateTime, message)
        {
            if(locations.Count() > 0 || lotNumbers.Count() > 0 || expirationDates.Count() > 0)
            {
                Reason = reason;
                Locations = locations.ToList();
                LotNumbers = lotNumbers.ToList();
                ExpirationDates = expirationDates.ToList();
            }
            else
            {
                throw new ArgumentException($"No recall identifier information (\"{nameof(locations)}\", \"{nameof(lotNumbers)}\", " +
                    $"\"{nameof(expirationDates)}\") was provided.");
            }
        }
    }
}
