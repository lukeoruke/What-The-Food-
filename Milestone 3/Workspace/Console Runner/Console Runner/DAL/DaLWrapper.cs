using Class1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner.DAL
{
    internal class DaLWrapper
    {
        Context context = new();
        public DaLWrapper()
        {

        }

        /////////////////////////////////////////////////////////////Accounts////////////////////////////////////////////////////////////////////////////////////////////////

        public bool userExists(string email)
        {
            if (context.accounts.Find(email) != null)
            {
                return true;
            }
            return false;
        }

        public Account getAccount(string email)
        {
            Account acc = context.accounts.Find(email);
            if (acc != null)
            {
                return acc;
            }
            else
            {
                throw new Exception("account not found exception");
            }

        }
        public bool addAccount(Account acc)
        {
            context.accounts.Add(acc);
            context.SaveChanges();
            return true;
        }
        public bool removeAccount(Account acc)
        {
            context.Remove(acc);
            context.SaveChanges();
            return true;
        }
        /////////////////////////////////////////////////////////////Permissions////////////////////////////////////////////////////////////////////////////////////////////////
        public bool hasPermission(string email, string permission)
        {
            if (context.permissions.Find(email, permission) != null)
            {
                return true;
            }
            return false ;
        }

    }
}
