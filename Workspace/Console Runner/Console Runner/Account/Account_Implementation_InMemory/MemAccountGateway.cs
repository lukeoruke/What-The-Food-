


namespace Console_Runner.AccountService
{
    public class MemAccountGateway : IAccountGateway
    {
        private List<Account> _memContextAccount;
        public MemAccountGateway()
        {
            _memContextAccount = new List<Account>();
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
                if(acc.UserID == -1)
                {
                    Random random = new Random();
                    acc.UserID = random.Next();
                    while(await AccountExistsAsync(acc.UserID))
                    {
                        acc.UserID = random.Next();
                    }
                }
                _memContextAccount.Add(acc);
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<Account?> GetAccountAsync(int UserID)
        {
            foreach (Account account in _memContextAccount)
            {
                if (account.UserID == UserID)
                {
                    return account;
                }
            }
            return null;
        }

        public int GetIDFromEmail(string email)
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
                await RemoveAccountAsync(acc);
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
                foreach (Account account in _memContextAccount)
                {
                    if (account.UserID == acc.UserID)
                    {
                        await RemoveAccountAsync(account);
                        await AddAccountAsync(acc);
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
