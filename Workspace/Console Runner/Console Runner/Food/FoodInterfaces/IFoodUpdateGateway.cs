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
        public Task<bool> AddFoodUpdateAsync(FoodUpdate foodUpdate, LogService? logger = null);
        public Task<List<FoodUpdate>> GetFoodUpdatesByBarcodeAsync(string barcode, LogService? logger = null);
        public Task<bool> UpdateFoodUpdateAsync(FoodUpdate foodUpdate, LogService? logger = null);
        public Task<bool> RemoveFoodUpdate(FoodUpdate foodUpdate, LogService? logger = null);
        public Task<bool> RemoveFoodUpdateByID(int updateID, LogService? logger = null);
    }
}
