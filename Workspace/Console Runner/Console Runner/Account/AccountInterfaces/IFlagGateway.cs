using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public interface IFlagGateway
    {
        public Task<List<FoodFlag>> GetNAccountFlags(int userID, int skip, int take, LogService? logService = null);
        public Task<bool> AccountHasFlagAsync(int userID, int ingredientID, LogService? logService = null);
        public Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID, LogService? logService = null);
        public Task<List<FoodFlag>> GetAllAccountFlagsAsync(int userID, LogService? logService = null);
        public Task<bool> AddFlagAsync(FoodFlag flag, LogService? logService = null);
    }
}
