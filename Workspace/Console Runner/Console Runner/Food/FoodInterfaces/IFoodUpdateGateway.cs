using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public interface IFoodUpdateGateway
    {
        /// <summary>
        /// Add a derived class of FoodUpdate to the database. Throws if attempting to add a FoodUpdate and not a derived type.
        /// </summary>
        /// <param name="foodUpdate">The FoodUpdate object to add to the database.</param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>True if the FoodUpdate was successfully added.</returns>
        public Task<bool> AddAsync(FoodUpdate foodUpdate, LogService? logService = null);
        
        /// <summary>
        /// Get all FoodUpdates that are associated with the given barcode from the database.
        /// </summary>
        /// <param name="barcode">The FoodItem barcode to get all associated FoodUpdates for.</param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>A List of all FoodUpdates associated with barcode on the database.</returns>
        public Task<List<FoodUpdate>> GetAllByBarcodeAsync(string barcode, LogService? logService = null);

        /// <summary>
        /// Update an existing FoodUpdate on the database with the same Id with the given FoodUpdate.
        /// </summary>
        /// <param name="foodUpdate">The FoodUpdate to update the one on the database with.</param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>True if the corresponding FoodUpdate in the database was updated.</returns>
        public Task<bool> UpdateAsync(FoodUpdate foodUpdate, LogService? logService = null);

        /// <summary>
        /// Remove the FoodUpdate with the same Id, FoodItemBarcode, and UpdateTime as the given FoodUpdate from the database if it exists.
        /// </summary>
        /// <param name="foodUpdate"></param>
        /// <param name="logService">The LogService to log actions with.</param>
        /// <returns>True if the corresponding FoodUpdate in the database was removed.</returns>
        public Task<bool> RemoveAsync(FoodUpdate foodUpdate, LogService? logService = null);
    }
}
