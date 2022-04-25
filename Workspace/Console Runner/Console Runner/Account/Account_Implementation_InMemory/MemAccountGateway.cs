using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public class MemAccountGateway : IAccountGateway
    {
        private List<Account> _memContextAccount;
        public MemAccountGateway()
        {
            _memContextAccount = new List<Account>();
        }

        public string getSalt(int userID, LogService? logService = null)
        {
            foreach(Account account in _memContextAccount)
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


        public int NumberOfAccounts(LogService? logService = null)
        {
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Retrieved total number of accounts");
            }
            return _memContextAccount.Count;
        }

        public async Task<bool> AccountExistsAsync(int userID, LogService? logService = null)
        {
            var toReturn = _memContextAccount.Find((acc => acc.UserID == userID)) != null;
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
                Random random = new Random();
                acc.UserID = random.Next(1,1000);
                while(await AccountExistsAsync(acc.UserID))
                {
                    acc.UserID = random.Next(1,01000);
                }
                _memContextAccount.Add(acc);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Created account for {acc.Email}");
                }
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<Account?> GetAccountAsync(int userID, LogService? logService = null)
        {
            foreach (Account account in _memContextAccount)
            {
                if (account.UserID == userID)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved account {userID}");
                    }
                    return account;
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Could not retrieve account {userID}");
            }
            return null;
        }

        public async Task<int> GetIDFromEmailIdAsync(string email, LogService? logService = null)
        {
            foreach (Account account in _memContextAccount)
            {
                if (account.Email == email)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved email for user {account.UserID}");
                    }
                    return account.UserID;
                }
            }
            return -1;
        }

        public async Task<bool> RemoveAccountAsync(Account acc, LogService? logService = null)
        {
            try
            {
                _memContextAccount.Remove(acc);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Removed account {acc.UserID}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAccountAsync(Account acc, LogService? logService = null)
        {
            try
            {
                for(int i = 0; i < _memContextAccount.Count; i++)
                {
                    if (_memContextAccount[i].UserID == acc.UserID)
                    {
                        _memContextAccount[i] = acc;
                        _memContextAccount[i].IsActive = false;
                        if (logService?.UserID != null)
                        {
                            _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                               $"Updated account {acc.UserID}");
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
