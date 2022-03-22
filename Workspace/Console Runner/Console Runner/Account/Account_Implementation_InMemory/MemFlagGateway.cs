

namespace Console_Runner.AccountService
{
    public class MemFlagGateway : IFlagGateway
    {
        private List<FoodFlag> _flagsListDB;
        
        public MemFlagGateway()
        {
            _flagsListDB = new List<FoodFlag>();
        }
        public async Task<bool> AccountHasFlagAsync(int userID, int ingredientID)
        {
            foreach(FoodFlag flag in _flagsListDB)
            {
                if(flag.UserID == userID && flag.IngredientID == ingredientID)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> AddFlagAsync(FoodFlag flag)
        {
            if (flag != null && !_flagsListDB.Contains(flag))
            {
                _flagsListDB.Add(flag);
                return true;
            }
            return false;
        }

        public List<FoodFlag> GetAllAccountFlags(int userID)
        {
            List<FoodFlag> accountFlags = new List<FoodFlag>();
            foreach (var flag in _flagsListDB)
            {
                if (flag.UserID == userID)
                {
                    accountFlags.Add(flag);
                }
            }
            return accountFlags;
        }

        public async Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID)
        {
            foreach (FoodFlag flag in _flagsListDB)
            {
                if (flag.UserID == userID && flag.IngredientID == ingredientID)
                {
                   _flagsListDB.Remove(flag);
                    return true;
                }
            }
            return false;
        }
    }
}
