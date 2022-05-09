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
        /// <summary>
        /// Gets the salt associated with a user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns>The salt associated with a user</returns>
        public string getSalt(int userID, LogService? logService = null)
        {
            foreach(var account in _efContext.Accounts)
            {
                if(account.UserID == userID)
                {
                    if (logService?.UserEmail != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved password salt for user {userID}");
                    }
                    return account.Salt;
                }
            }
            if (logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Debug, Category.DataStore, DateTime.Now,
                                                   $"Password salt for user {userID} does not exist");
            }
            return null;
        }

        /// <summary>
        /// Verify whether an Account object exists in the database with the provided ID.
        /// </summary>
        /// <param name="UserID">UserID to search for</param>
        /// <returns>True if the searched account exists, false otherwise.</returns>
        public async Task<bool>AccountExistsAsync(int userID, LogService? logService = null)
        {
            var toReturn = await _efContext.Accounts.FindAsync(userID) != null;
            if (logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Checked if account {userID} exists - {toReturn}");
            }
            return toReturn;
        }
        /// <summary>
        /// Add an Account object to the database.
        /// </summary>
        /// <param name="acc"> Account object to add to the database</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public async Task<bool> AddAccountAsync(Account acc, LogService? logService = null)
        {
            try
            {
                await _efContext.Accounts.AddAsync(acc);
                await _efContext.SaveChangesAsync();
                if (logService?.UserEmail != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Created account for {acc.Email}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Retrieve an Account object from the database.
        /// </summary>
        /// <param name="UserID">UserID  to retrieve</param>
        /// <returns>Account object with the provided AccountID assuming it exists, otherwise null if the account does not exist.</returns>
        public async Task<Account?> GetAccountAsync(int userID, LogService? logService = null)
        {
            foreach(var acc in _efContext.Accounts)
            {
                if(acc.UserID == userID)
                {
                    if (logService?.UserEmail != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved account {userID}");
                    }
                    return acc;
                }
            }
            if (logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Could not retrieve account {userID}");
            }
            throw new Exception("NO ACCOUNT WAS FOUND WITH USERID: " + userID.ToString());
        }
        /// <summary>
        /// Remove an Account object from the database.
        /// </summary>
        /// <param name="acc">The Account object being removed from the database</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public async Task<bool> RemoveAccountAsync(Account acc, LogService? logService = null)
        {
            try
            {
                if (await AccountExistsAsync(acc.UserID))
                {
                    _efContext.Remove(acc);
                    _efContext.SaveChanges();
                    if (logService?.UserEmail != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
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
        /// <summary>
        /// Update an Account object in the database. Modify the account object, then pass it into this method. The corresponding object in the database will be updated accordingly.
        /// </summary>
        /// <param name="acc">The Account object with modified parameters</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public async Task<bool> UpdateAccountAsync(Account acc, LogService? logService = null)
        {
            try
            {
                //TODO NEED TO VERIFY THIS WONT SKIP OVER UPDATE BEFORE GOING TO SAVE CHANGES. 
                _efContext.Accounts.Update(acc);
                await _efContext.SaveChangesAsync(true);
                if (logService?.UserEmail != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Updated account {acc.UserID}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Gets a users ID from their email address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="logService"></param>
        /// <returns>the ID associated with a specifici email</returns>
        public async Task<int> GetIDFromEmailIdAsync(string email, LogService? logService = null)
        {
            var userEmail = _efContext.Accounts.Where(r => r.Email == email);
            if(userEmail.Count() == 0)
            {
                throw new ArgumentException("EFAccountGateway.GetIDFromEmailIdAsync: Invalid email");
            }
            List<Account> tempAcc = await userEmail.ToListAsync();
            if (logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Retrieved email for user {tempAcc[0].UserID}");
            }
            return tempAcc[0].UserID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logService"></param>
        /// <returns>returns the number of accounts</returns>
        public int NumberOfAccounts(LogService? logService = null)
        {
            int counter = 0;
            foreach(var account in _efContext.Accounts)
            {
                counter++;
            }
            if (logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Retrieved total number of accounts");
            }
            return counter;
        }

    }
}
