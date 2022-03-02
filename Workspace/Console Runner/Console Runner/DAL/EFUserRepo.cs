using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class1;
using User;

namespace Console_Runner.DAL
{
    public class EFUserRepo : IAccountRepo
    {
        private Context EFContext = new();
        public EFUserRepo()
        {

        }

        public bool AccountExists(string email)
        {
            if (EFContext.Accounts.Find(email) != null)
            {
                return true;
            }
            return false;
        }

        public bool AddAccount(Account acc)
        {
            try
            {
                EFContext.Accounts.Add(acc);
                EFContext.SaveChanges();
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
                Account acc = EFContext.Accounts.Find(email);
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
                    EFContext.Remove(acc);
                    EFContext.SaveChanges();
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
                EFContext.Accounts.Update(acc);
                EFContext.SaveChanges(true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
