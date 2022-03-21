using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console_Runner.AccountService
{
    public class MemAccountFunctions : IAccountGateway
    {
        private List<Account> accountsList;
        
        public MemAccountFunctions()
        {
            accountsList = new List<Account>();
        }
        public bool AccountExistsAsync(int userID)
        {
            for (int i = 0; i < accountsList.Count; i++)
            {
                if (userID == accountsList[i].UserID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddAccountAsync(Account acc)
        {
            accountsList.Add(acc);

            return true;
        }

        public Account? GetAccount(int AccountID)
        {
            for (int i = 0; i < accountsList.Count; i++)
            {
                if (accountsList[i].AccountID == AccountID)
                {
                    return accountsList[i];
                }
            }
            return null;
        }

        public bool RemoveAccount(Account acc)
        {
            for (int i = 0; i < accountsList.Count; i++)
            {
                if (accountsList[i].AccountID == acc.AccountID)
                {
                    accountsList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateAccount(Account acc)
        {
            for (int i = 0; i < accountsList.Count; i++)
            {
                if (accountsList[i].AccountID == acc.AccountID)
                {
                    accountsList[i] = acc;
                    return true;
                }
            }
            return false;
        }
    }
}
