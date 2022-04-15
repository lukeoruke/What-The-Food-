

namespace Console_Runner.AccountService
{
    public interface IFlagGateway
    {
        public Task<List<FoodFlag>> GetNAccountFlags(int userID, int skip, int take);
        public Task<bool> AccountHasFlagAsync(int userID, int ingredientID);
        public Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID);
        public Task<List<FoodFlag>> GetAllAccountFlagsAsync(int userID);
        public Task<bool> AddFlagAsync(FoodFlag flag);
    }
}
