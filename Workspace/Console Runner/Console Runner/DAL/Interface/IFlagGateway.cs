using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;

namespace Console_Runner.DAL
{
    public interface IFlagGateway
    {
        public bool AccountHasFlag(string email, string flag);
        public bool RemoveFoodFlag(string email, string flag);
        public List<FoodFlag> GetAllAccountFlags(string email);
        public bool AddFlag(FoodFlag flag);
    }
}
