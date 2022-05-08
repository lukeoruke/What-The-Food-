using Console_Runner.AccountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Account.Account_Implementation_InMemory
{
    public class MemActiveSessionTracker : IActiveSessionTrackerGateway
    {
        public Task<int> GetActiveUserAsync(string jwt)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTokenFromUserID(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveToken(string jwt)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartSessionAsync(int userId, string jwt)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateToken(string jwt)
        {
            throw new NotImplementedException();
        }
    }
}
