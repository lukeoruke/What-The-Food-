using Console_Runner.Account.AccountInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.AccountService
{
    public class MemNews: IAccountNews
    {
        private ContextAccountDB _efContext;

        //constructor
        private List<Account> _newsDB;

        public MemNews()
        {
            _newsDB = new List<Account>();
        }

        public async Task<bool> DecrementBias(int userID)
        {
            //Find and assign instance of userID within database
            foreach (Account account in _newsDB)
            {
                if (account.UserID == userID)
                {
                    if (account.NewsBias > 1)
                    {
                        account.NewsBias = account.NewsBias - 1;
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Did not update because it is at min \n");
                    return false;
                }
            }
            return false;

        }
        public async Task<bool> IncrementBias(int userID)
        {
            foreach (Account account in _newsDB)
            {
                if (account.UserID == userID)
                {
                    if (account.NewsBias < 4)
                    {
                        account.NewsBias = account.NewsBias - 1;
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Did not update because it is at max \n");
                    return false;
                }
            }
            return false;
        }

        public async Task<int> GetBias(int userID)
        {
            foreach (Account account in _newsDB)
            {
                if (account.UserID == userID)
                {
                    return account.NewsBias;
                }
            }
            return -1;
        }
    }
}
