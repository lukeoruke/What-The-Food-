﻿

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

        public async Task<List<FoodFlag>> GetAllAccountFlagsAsync(int userID)
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

        public async Task<List<FoodFlag>> GetNAccountFlagsAsync(int userID, int skip, int take)
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
            return subList;
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
