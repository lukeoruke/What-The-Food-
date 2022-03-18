using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console_Runner.AccountDB
{
    public class EFAccountFunctions : IAccountFunctions
    {

        private Context _efContext;

        public EFAccountFunctions()
        {
            _efContext = new Context();
        }


        /// <summary>
        /// Checks if an account exists
        /// </summary>
        /// <param name="AccountID">The ID of the account being checked</param>
        /// <returns>true if account exists false otherwise</returns>
        public bool AccountExists(int AccountID)
        {
            if (_efContext.Accounts.Find(AccountID) != null)
            {
                return true;
            }

            return false;
        }

        public bool AddAccount(Account acc)
        {
            try
            {
                _efContext.Accounts.Add(acc);
                _efContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Account? GetAccount(int AccountID)
        {
            try
            {
                Account? acc = _efContext.Accounts.Find(AccountID);
                if (acc != null)
                {
                    return acc;
                }
                else
                {
                    throw new Exception("account not found exception");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool RemoveAccount(Account acc)
        {
            try
            {
                if (AccountExists(acc.AccountID))
                {
                    _efContext.Remove(acc);
                    _efContext.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateAccount(Account acc)
        {
            try
            {
                _efContext.Accounts.Update(acc);
                _efContext.SaveChanges(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
