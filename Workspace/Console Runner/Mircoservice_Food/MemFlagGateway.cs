using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public class MemFlagGateway : IFlagGateway
    {
        private List<FoodFlag> _flagsListDB;
        
        public MemFlagGateway()
        {
            _flagsListDB = new List<FoodFlag>();
        }
        public async Task<bool> AccountHasFlagAsync(int userID, int ingredientID, LogService? logService = null)
        {
            foreach(FoodFlag flag in _flagsListDB)
            {
                if(flag.UserID == userID && flag.IngredientID == ingredientID)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Retrieved food flag for user {userID} and ingredient {ingredientID} - exists? {true}");
                    }
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> AddFlagAsync(FoodFlag flag, LogService? logService = null)
        {
            if (flag != null && !_flagsListDB.Contains(flag))
            {
                _flagsListDB.Add(flag);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created food flag for user {flag.UserID} and ingredient {flag.IngredientID}");
                }
                return true;
            }
            return false;
        }

        public List<FoodFlag> GetAllAccountFlags(int userID, LogService? logService = null)
        {
            List<FoodFlag> accountFlags = new List<FoodFlag>();
            foreach (var flag in _flagsListDB)
            {
                if (flag.UserID == userID)
                {
                    accountFlags.Add(flag);
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved all food flags for user {userID}");
            }
            return accountFlags;
        }

        public async Task<List<FoodFlag>> GetAllAccountFlagsAsync(int userID, LogService? logService = null)
        {
            List<FoodFlag> accountFlags = new List<FoodFlag>();
            foreach (var flag in _flagsListDB)
            {
                if (flag.UserID == userID)
                {
                    accountFlags.Add(flag);
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved all food flags for user {userID}");
            }
            return accountFlags;
        }

        public async Task<List<FoodFlag>> GetNAccountFlagsAsync(int userID, int skip, int take, LogService? logService = null)
        {
            List<FoodFlag> accountFlags = new List<FoodFlag>();

            for(int i = 0; i < _flagsListDB.Count; i++)
            {
                if(_flagsListDB[i].UserID == userID)
                {
                    accountFlags.Add(_flagsListDB[i]);
                }
            }
            List<FoodFlag> subList = new List<FoodFlag>();
            for (int i = skip; i < accountFlags.Count; i++)
            {
                if(take == 0)
                {
                    return subList;
                }
                subList.Add(accountFlags[i]);
                take--;

            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved {take} food flags after {skip} for user {userID}");
            }
            return subList;
        }

        public async Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID, LogService? logService = null)
        {
            foreach (FoodFlag flag in _flagsListDB)
            {
                if (flag.UserID == userID && flag.IngredientID == ingredientID)
                {
                   _flagsListDB.Remove(flag);
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Removed food flag for user {userID} and ingredient {ingredientID}");
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
