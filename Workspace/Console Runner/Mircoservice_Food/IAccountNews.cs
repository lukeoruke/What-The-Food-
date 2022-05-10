using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Account.AccountInterfaces
{
    //define function calls to EF News which will make a direct call to the database
    public interface IAccountNews
    {
        public Task<bool> IncrementBias(int userID);
        public Task<bool> DecrementBias(int userID);
        public Task<int> GetBias(int userID);
    }
}

