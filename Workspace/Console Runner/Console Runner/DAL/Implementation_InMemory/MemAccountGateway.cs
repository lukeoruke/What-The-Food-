using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.User_Management;

namespace Console_Runner.DAL
{
    public class MemAccountGateway : IAccountGateway
    {
        private List<Account> accountsList;
        
        public MemAccountGateway()
        {
            accountsList = new List<Account>();
        }
        public bool AccountExists(string email)
        {
            for (int i = 0; i < accountsList.Count; i++)
            {
                if (email == accountsList[i].Email)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddAccount(Account acc)
        {
            accountsList.Add(acc);

            return true;
        }

        public Account? GetAccount(string email)
        {
            for (int i = 0; i < accountsList.Count; i++)
            {
                if (accountsList[i].Email == email)
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
                if (accountsList[i].Email == acc.Email)
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
                if (accountsList[i].Email == acc.Email)
                {
                    accountsList[i] = acc;
                    return true;
                }
            }
            return false;
        }
    }
}
