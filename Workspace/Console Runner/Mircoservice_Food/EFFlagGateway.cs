using Microsoft.EntityFrameworkCore;
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
        /// <summary></summary>
        /// <param name="userID">The user whos ID's are being retrieved</param>
        /// <param name="ingredientID">The ingredients ID</param>
        /// <returns>true if the user has a flag associated with the provided ingredient ID</returns>
        public async Task<bool> AccountHasFlagAsync(int userID, int ingredientID, LogService? logService = null)
        {
            FoodFlag foodFlag = new(userID, ingredientID);
            var toReturn = await _efContext.FoodFlags.FindAsync(foodFlag) != null;
            if(logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now, 
                    $"Retrieved food flag for user {userID} and ingredient {ingredientID} - exists? {toReturn}");
            }
            return toReturn;
        }
        /// <summary>
        /// adds a flag to the users account
        /// </summary>
        /// <param name="flag">The flag to be added</param>
        /// <returns>true if successful false otherwise</returns>
        public async Task<bool> AddFlagAsync(FoodFlag flag, LogService? logService = null)
        {
            try
            {
                await _efContext.FoodFlags.AddAsync(flag);
                await _efContext.SaveChangesAsync();
                if (logService?.UserEmail != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created food flag for user {flag.UserID} and ingredient {flag.IngredientID}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Gets N(take) flags that belong to the userID provided while skipping over first m(skip) results. 
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved</param>
        /// <param name="skip">The number of entries to skip before pulling</param>
        /// <param name="take">The number of entries to return</param>
        /// <returns>A list containing the food flags associated with the user</returns>
        public async Task<List<FoodFlag>> GetNAccountFlagsAsync(int userID, int skip, int take, LogService? logService = null)
        {
            List<FoodFlag> results =  await _efContext.FoodFlags.Where(x => x.UserID == userID).
                OrderBy(x => x.IngredientID).Skip(skip).Take(take).ToListAsync();
            if (logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved {take} food flags after {skip} associated with user {userID}");
            }
            return results;
        }
        /// <summary>
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved</param>
        /// <returns>A list of all flags associated with the users account</returns>
        public async Task<List<FoodFlag>> GetAllAccountFlagsAsync(int userID, LogService? logService = null)
        {
            List<FoodFlag> results = await _efContext.FoodFlags.Where(x => x.UserID == userID).ToListAsync();
            if (logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved all food flags for user {userID}");
            }
            return results;
        }
        /// <summary>
        /// Removes a flag from the users account
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved </param>
        /// <param name="ingredientID">The ingredients ID</param>
        /// <returns></returns>
        public async Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID, LogService? logService = null)
        {
            try
            {
                FoodFlag foodFlag = new(userID, ingredientID);
                _efContext.FoodFlags.Remove(foodFlag);
                await _efContext.SaveChangesAsync();
                if (logService?.UserEmail != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Removed food flag for user {userID} and ingredient {ingredientID}");
                }
                return true;
            }
            catch(Exception ex)
            {
                if (logService?.UserEmail != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Cannot remove food flag for user {userID} and ingredient {ingredientID}. Unknown error: {ex.Message}");
                }
                throw new Exception("Remove flag failed: " + ex.ToString());
            }
        }
        public async Task<bool> RemoveFoodFlagAsync(FoodFlag foodFlag, LogService? logService = null)
        {
            try
            {

                _efContext.FoodFlags.Remove(foodFlag);
                await _efContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw new Exception("Remove flag failed: " + ex.ToString());
            }
        }
    }
}
