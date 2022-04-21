using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public class MemFoodUpdateGateway : IFoodUpdateGateway
    {
        private static int _idCount;
        private List<FoodUpdate> _foodUpdateDB;
         
        public MemFoodUpdateGateway()
        {
            _foodUpdateDB = new List<FoodUpdate>();
        }

        /// <summary>
        /// Add a FoodUpdate to the database.
        /// </summary>
        /// <param name="foodUpdate">The FoodUpdate object to add to the database.</param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>True if the FoodUpdate was successfully added.</returns>
        public async Task<bool> AddAsync(FoodUpdate foodUpdate, LogService? logService = null)
        {
            Thread.Sleep(100);
            foodUpdate.Id = _idCount++;
            _foodUpdateDB.Add(foodUpdate);
            return true;
        }

        /// <summary>
        /// Get all FoodUpdates that are associated with the given barcode from the database defined in ContextFoodDB.
        /// </summary>
        /// <param name="barcode">The FoodItem barcode to get all associated FoodUpdates for.</param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>A List of all FoodUpdates associated with barcode on the database.</returns>
        public async Task<List<FoodUpdate>> GetAllByBarcodeAsync(string barcode, LogService? logService = null)
        {
            Thread.Sleep(100);
            List<FoodUpdate> updateList = _foodUpdateDB.FindAll(foodUpdate => foodUpdate.FoodItemBarcode == barcode);
            return updateList;
        }

        /// <summary>
        /// Remove the given FoodUpdate from the database if it exists.
        /// </summary>
        /// <param name="foodUpdate"></param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>True if the corresponding FoodUpdate in the database was removed.</returns>
        public async Task<bool> RemoveAsync(FoodUpdate foodUpdate, LogService? logService = null)
        {
            Thread.Sleep(100);
            _foodUpdateDB.RemoveAll(fu => fu.Id == foodUpdate.Id ||
                                          (fu.FoodItemBarcode == foodUpdate.FoodItemBarcode && fu.UpdateTime == foodUpdate.UpdateTime));
            return true;
        }

        /// <summary>
        /// Update an existing FoodUpdate on the database defined in ContextFoodDB with the same Id with the given FoodUpdate.
        /// </summary>
        /// <param name="foodUpdate">The FoodUpdate to update the one on the database with.</param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>True if the corresponding FoodUpdate in the database was updated.</returns>
        public async Task<bool> UpdateAsync(FoodUpdate foodUpdate, LogService? logService = null)
        {
            Thread.Sleep(100);
            int index = _foodUpdateDB.FindIndex(fu => fu.Id == foodUpdate.Id ||
                                                      (fu.FoodItemBarcode == foodUpdate.FoodItemBarcode && fu.UpdateTime == foodUpdate.UpdateTime));
            if (index == -1)
            {
                return false;
            }
            _foodUpdateDB[index] = foodUpdate;
            return true;
        }
    }
}
