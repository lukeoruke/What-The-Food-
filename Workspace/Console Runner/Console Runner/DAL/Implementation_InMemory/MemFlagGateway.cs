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
        private List<FoodFlag> flagsList;

        public MemFlagGateway()
        {
            flagsList = new List<FoodFlag>();
        }
        public bool AccountHasFlag(string email, string ingredientID)
        {
            FoodFlag flag = new FoodFlag(email, ingredientID);
            return flagsList.Contains(flag);
        }

        public bool AddFlag(FoodFlag flag)
        {
            if (flag != null && !flagsList.Contains(flag))
            {
                flagsList.Add(flag);
                return true;
            }
            return false;
        }

        public List<FoodFlag> GetAllAccountFlags(string email)
        {
            List<FoodFlag> flagList = new List<FoodFlag>();
            foreach (var flag in flagsList)
            {
                if (flag.accountEmail == email)
                {
                    flagList.Add(flag);
                }
            }
            return flagList;
        }

        public bool RemoveFoodFlag(string email, string ingredientID)
        {
            if (AccountHasFlag(email, ingredientID))
            {
                FoodFlag foodFlag = new(email, ingredientID);
                flagsList.Remove(foodFlag);
                return true;
            }
            return false;
        }
    }
}
