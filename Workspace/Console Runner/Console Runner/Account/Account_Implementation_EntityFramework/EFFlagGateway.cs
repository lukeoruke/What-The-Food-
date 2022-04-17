using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Console_Runner.Logging;

namespace Console_Runner.AccountService
{ 
    public class EFFlagGateway : IFlagGateway
    {
        private ContextAccountDB _efContext;

        public EFFlagGateway()
        {
            _efContext = new ContextAccountDB();
        }
        public async Task<bool> AccountHasFlagAsync(int userID, int ingredientID, LogService? logService = null)
        {
            FoodFlag foodFlag = new(userID, ingredientID);
            var toReturn = await _efContext.FoodFlags.FindAsync(foodFlag) != null;
            if(logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now, 
                    $"Retrieved food flag for user {userID} and ingredient {ingredientID} - exists? {toReturn}");
            }
            return toReturn;
        }

        public async Task<bool> AddFlagAsync(FoodFlag flag, LogService? logService = null)
        {
            try
            {
                await _efContext.FoodFlags.AddAsync(flag);
                await _efContext.SaveChangesAsync();
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created food flag for user {flag.UserID} and ingredient {flag.IngredientID}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<FoodFlag> GetAllAccountFlags(int userID, LogService? logService = null)
        {
            List<FoodFlag> flagList = new List<FoodFlag>();
            foreach (var flag in _efContext.FoodFlags)
            {
                if (flag.UserID == userID)
                {
                    flagList.Add(flag);
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved all food flags for user {userID}");
            }
            return flagList;
        }

        public async Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID, LogService? logService = null)
        {
            if (await AccountHasFlagAsync(userID, ingredientID))
            {
                FoodFlag foodFlag = new(userID, ingredientID);
                _efContext.FoodFlags.Remove(foodFlag);
                await _efContext.SaveChangesAsync();
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Removed food flag for user {userID} and ingredient {ingredientID}");
                }
                return true;
            }
            return false;
        }
    }
}
