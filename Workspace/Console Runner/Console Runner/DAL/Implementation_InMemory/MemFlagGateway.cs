using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;

namespace Console_Runner.DAL
{
    public class MemFlagGateway : IFlagGateway
    {
        private List<FoodFlag> _flagsListDB;
        
        public MemFlagGateway()
        {
            _flagsListDB = new List<FoodFlag>();
        }
        public bool AccountHasFlag(string email, string ingredientID)
        {
            for(int i = 0; i < _flagsListDB.Count; i++)
            {
                if(_flagsListDB[i].AccountEmail == email && _flagsListDB[i].IngredientID == ingredientID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddFlag(FoodFlag flag)
        {
            if (flag != null && !_flagsListDB.Contains(flag))
            {
                _flagsListDB.Add(flag);
                return true;
            }
            return false;
        }

        public List<FoodFlag> GetAllAccountFlags(string email)
        {
            List<FoodFlag> accountFlags = new List<FoodFlag>();
            foreach (var flag in _flagsListDB)
            {
                if (flag.AccountEmail == email)
                {
                    accountFlags.Add(flag);
                }
            }
            return accountFlags;
        }

        public bool RemoveFoodFlag(string email, string ingredientID)
        {
            for (int i = 0; i < _flagsListDB.Count; i++)
            {
                if (_flagsListDB[i].AccountEmail == email && _flagsListDB[i].IngredientID == ingredientID)
                {
                   _flagsListDB.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}
