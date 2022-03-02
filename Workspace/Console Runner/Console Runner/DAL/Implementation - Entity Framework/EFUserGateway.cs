using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class1;
using User;

namespace Console_Runner.DAL
{
    public class EFUserGateway : IAccountGateway
    {
        private Context _efContext;

        public EFUserGateway(Context dbcontext)
        {
            _efContext = dbcontext;
        }

        public bool AccountExists(string email)
        {
            if (_efContext.Accounts.Find(email) != null)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public Account GetAccount(string email)
        {
            try
            {
                Account acc = _efContext.Accounts.Find(email);
                if (acc != null)
                {
                    return acc;
                }
                else
                {
                    throw new Exception("account not found exception");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool RemoveAccount(Account acc)
        {
            try
            {
                if (AccountExists(acc.Email))
                {
                    _efContext.Remove(acc);
                    _efContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
