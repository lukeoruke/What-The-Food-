using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public class EFFoodUpdateGateway : IFoodUpdateGateway
    {
        public EFFoodUpdateGateway()
        {

        }

        /// <summary>
        /// Add a FoodUpdate to the database defined in ContextFoodDB.
        /// </summary>
        /// <param name="foodUpdate">The FoodUpdate object to add to the database.</param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>True if the FoodUpdate was successfully added.</returns>
        public async Task<bool> AddAsync(FoodUpdate foodUpdate, LogService? logService = null)
        {
            if(foodUpdate.GetType() == typeof(FoodUpdate))
            {
                throw new ArgumentException($"{nameof(foodUpdate)} is a FoodUpdate and not a derived type.");
            }
            using ContextFoodDB contextFoodDB = new ContextFoodDB();
            contextFoodDB.Entry(foodUpdate.FoodItem).State = EntityState.Unchanged;
            await contextFoodDB.AddAsync(foodUpdate);
            await contextFoodDB.SaveChangesAsync();
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Data, DateTime.Now,
                        $"Created {foodUpdate.GetType} FoodUpdate for FoodItem {foodUpdate.FoodItem.Barcode} to database.");
            }
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
            using ContextFoodDB contextFoodDB = new ContextFoodDB();
            List<FoodUpdate> foodUpdates = await contextFoodDB.FoodUpdates.Where(foodUpdate => foodUpdate.FoodItemBarcode == barcode).ToListAsync();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Data, DateTime.Now,
                        $"Retrieved all FoodUpdates for FoodItem {barcode} from database ({foodUpdates.Count} instances).");
            }
            return foodUpdates;
        }

        /// <summary>
        /// Remove the FoodUpdate with the same Id, FoodItemBarcode, and UpdateTime as the given FoodUpdate from the database if it exists.
        /// </summary>
        /// <param name="foodUpdate"></param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>True if the corresponding FoodUpdate in the database was removed.</returns>
        public async Task<bool> RemoveAsync(FoodUpdate foodUpdate, LogService? logService = null)
        {
            using ContextFoodDB contextFoodDB = new ContextFoodDB();
            contextFoodDB.Remove(foodUpdate);
            await contextFoodDB.SaveChangesAsync();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Data, DateTime.Now,
                        $"Deleted FoodUpdate {foodUpdate.Id} from the database.");
            }
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
            using ContextFoodDB contextFoodDB = new ContextFoodDB();
            contextFoodDB.Update(foodUpdate);
            await contextFoodDB.SaveChangesAsync();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.Data, DateTime.Now,
                        $"Updated FoodUpdate {foodUpdate.Id} in the database.");
            }
            return true;
        }
    }
}
