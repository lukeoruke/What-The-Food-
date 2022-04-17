using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public interface IFlagGateway
    {
        public Task<bool> AccountHasFlagAsync(int userID, int ingredientID, LogService? logService = null);
        public Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID, LogService? logService = null);
        public List<FoodFlag> GetAllAccountFlags(int userID, LogService? logService = null);
        public Task<bool> AddFlagAsync(FoodFlag flag, LogService? logService = null);
    }
}
