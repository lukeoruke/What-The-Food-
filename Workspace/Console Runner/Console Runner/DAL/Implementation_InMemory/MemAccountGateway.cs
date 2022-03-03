using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner.DAL
{
    public class MemAccountGateway : IAccountGateway
    {
        public bool AccountExists(string email)
        {
            throw new NotImplementedException();
        }

        public bool AddAccount(Account acc)
        {
            throw new NotImplementedException();
        }

        public Account? GetAccount(string email)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAccount(Account acc)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAccount(Account acc)
        {
            throw new NotImplementedException();
        }
    }
}
