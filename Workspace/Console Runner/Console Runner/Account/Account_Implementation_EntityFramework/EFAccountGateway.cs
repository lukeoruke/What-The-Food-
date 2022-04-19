using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public class EFAccountGateway : IAccountGateway
    {

        private readonly ContextAccountDB _efContext;

        public EFAccountGateway()
        {
            _efContext = new ContextAccountDB();
        }

        public string getSalt(int userID, LogService? logService = null)
        {
            foreach(var account in _efContext.Accounts)
            {
                if(account.UserID == userID)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved password salt for user {userID}");
                    }
                    return account.salt;
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                                                   $"Password salt for user {userID} does not exist");
            }
            return null;
        }

        /// <summary>
        /// Checks if an account exists
        /// </summary>
        /// <param name="userID">The ID of the account being checked</param>
        /// <returns>true if account exists false otherwise</returns>
        public async Task<bool>AccountExistsAsync(int userID, LogService? logService = null)
        {
            var toReturn = await _efContext.Accounts.FindAsync(userID) != null;
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Checked if account {userID} exists - {toReturn}");
            }
            return toReturn;
        }

        public async Task<bool> AddAccountAsync(Account acc, LogService? logService = null)
        {
            try
            {
                await _efContext.Accounts.AddAsync(acc);
                await _efContext.SaveChangesAsync();
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Created account for {acc.Email}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Account?> GetAccountAsync(int userID, LogService? logService = null)
        {
            foreach(var acc in _efContext.Accounts)
            {
                if(acc.UserID == userID)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved account {userID}");
                    }
                    return acc;
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Could not retrieve account {userID}");
            }
            throw new Exception("NO ACCOUNT WAS FOUND WITH USERID: " + userID.ToString());
            /*

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
                            throw new Exception("EN ERROR OCCURED DURING METHOD CALL");
                        }*/
        }

        public async Task<bool> RemoveAccountAsync(Account acc, LogService? logService = null)
        {
            try
            {
                if (await AccountExistsAsync(acc.UserID))
                {
                    _efContext.Remove(acc);
                    _efContext.SaveChanges();
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Removed account {acc.UserID}");
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAccountAsync(Account acc, LogService? logService = null)
        {
            try
            {
                //TODO NEED TO VERIFY THIS WONT SKIP OVER UPDATE BEFORE GOING TO SAVE CHANGES. 
                _efContext.Accounts.Update(acc);
                await _efContext.SaveChangesAsync(true);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Updated account {acc.UserID}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> GetIDFromEmailIdAsync(string email, LogService? logService = null)
        {
            var userEmail = _efContext.Accounts.Where(r => r.Email == email);
            List<Account> tempAcc = await userEmail.ToListAsync();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Retrieved email for user {tempAcc[0].UserID}");
            }
            return tempAcc[0].UserID;
        }

        public int NumberOfAccounts(LogService? logService = null)
        {
            int counter = 0;
            foreach(var account in _efContext.Accounts)
            {
                counter++;
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Retrieved total number of accounts");
            }
            return counter;
        }

    }
}
