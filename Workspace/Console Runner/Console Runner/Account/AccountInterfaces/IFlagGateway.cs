

namespace Console_Runner.AccountService
{
    public interface IFlagGateway
    {
        public Task<bool> AccountHasFlagAsync(int userID, int ingredientID);
        public Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID);
        public List<FoodFlag> GetAllAccountFlags(int userID);
        public Task<bool> AddFlagAsync(FoodFlag flag);
    }
}
