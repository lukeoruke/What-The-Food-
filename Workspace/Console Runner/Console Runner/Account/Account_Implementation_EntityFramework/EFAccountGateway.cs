using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console_Runner.AccountService
{
    public class EFAccountGateway : IAccountGateway
    {

        private readonly ContextAccountDB _efContext;

        public EFAccountGateway()
        {
            _efContext = new ContextAccountDB();
        }

        public string? GetSalt(int userID)
        {
            foreach(var account in _efContext.Accounts)
            {
                if(account.UserID == userID)
                {
                    return account.salt;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if an account exists
        /// </summary>
        /// <param name="UserID">The ID of the account being checked</param>
        /// <returns>true if account exists false otherwise</returns>
        public async Task<bool>AccountExistsAsync(int UserID)
        {
            return await _efContext.Accounts.FindAsync(UserID) != null;
        }

        public async Task<bool> AddAccountAsync(Account acc)
        {
            try
            {
               await _efContext.Accounts.AddAsync(acc);
               await _efContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Account?> GetAccountAsync(int UserID)
        {
            try
            {
                Account? acc = await _efContext.Accounts.FindAsync(UserID);
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

        public async Task<bool> RemoveAccountAsync(Account acc)
        {
            try
            {
                if (await AccountExistsAsync(acc.UserID))
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

        public async Task<bool> UpdateAccountAsync(Account acc)
        {
            try
            {
                //TODO NEED TO VERIFY THIS WONT SKIP OVER UPDATE BEFORE GOING TO SAVE CHANGES. 
                _efContext.Accounts.Update(acc);
                await _efContext.SaveChangesAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetIDFromEmail(string email)
        {
            var userEmail = _efContext.Accounts.Where(r => r.Email == email).FirstOrDefault();
            return userEmail?.UserID ?? -1;
        }

        public int NumberOfAccounts()
        {
            int counter = 0;
            foreach(var account in _efContext.Accounts)
            {
                counter++;
            }
            return counter;
        }

    }
}
