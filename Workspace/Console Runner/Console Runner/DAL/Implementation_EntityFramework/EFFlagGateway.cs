using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;

namespace Console_Runner.DAL
{
    public class EFFlagGateway : IFlagGateway
    {
        private Context _efContext;

        public EFFlagGateway(Context dbContext)
        {
            _efContext = dbContext;
        }
        public bool AccountHasFlag(string email, string flag)
        {
            FoodFlag foodFlag = new(email, flag);
            return _efContext.FoodFlags.Find(foodFlag) != null;
        }

        public bool AddFlag(FoodFlag flag)
        {
            try
            {
                _efContext.FoodFlags.Add(flag);
                _efContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<FoodFlag> GetAllAccountFlags(string email)
        {
            List<FoodFlag> flagList = new List<FoodFlag>();
            foreach (var flag in _efContext.FoodFlags)
            {
                if (flag.accountEmail == email)
                {
                    flagList.Add(flag);
                }
            }
            return flagList;
        }

        public bool RemoveFoodFlag(string email, string flag)
        {
            if (AccountHasFlag(email, flag))
            {
                FoodFlag foodFlag = new(email, flag);
                _efContext.FoodFlags.Remove(foodFlag);
                _efContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
