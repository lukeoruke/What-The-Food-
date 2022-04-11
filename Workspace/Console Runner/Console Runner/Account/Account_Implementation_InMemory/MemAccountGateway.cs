


namespace Console_Runner.AccountService
{
    public class MemAccountGateway : IAccountGateway
    {
        private List<Account> _memContextAccount;
        public MemAccountGateway()
        {
            _memContextAccount = new List<Account>();
        }

        public string getSalt(int userID)
        {
            foreach(Account account in _memContextAccount)
            {
                if(account.UserID == userID)
                {
                    return account.salt;
                }
            }
            return null;
        }


        public int NumberOfAccounts()
        {
            return _memContextAccount.Count;
        }
        public async Task<bool> AccountExistsAsync(int UserID)
        {
            foreach (Account account in _memContextAccount)
            {
                if(account.UserID == UserID)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> AddAccountAsync(Account acc)
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
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<Account?> GetAccountAsync(int userID)
        {
            foreach (Account account in _memContextAccount)
            {
                if (account.UserID == userID)
                {
                    return account;
                }
            }
            return null;
        }

        public async Task<int> GetIDFromEmail(string email)
        {
            foreach (Account account in _memContextAccount)
            {
                if (account.Email == email)
                {
                    return account.UserID;
                    
                }
            }
            return -1;
        }

        public async Task<bool> RemoveAccountAsync(Account acc)
        {
            try
            {
                _memContextAccount.Remove(acc);
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAccountAsync(Account acc)
        {
            try
            {
                for(int i = 0; i < _memContextAccount.Count; i++)
                {
                    if (_memContextAccount[i].UserID == acc.UserID)
                    {
                        _memContextAccount[i] = acc;

                        _memContextAccount[i].IsActive = false;

                        return true;
                    }
                }
                return false;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
