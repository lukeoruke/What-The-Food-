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
            try
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
            }catch (Exception ex)
            {
                return null;
            }
          

        }
        public bool addAccount(Account acc)
        {
            try
            {
                context.accounts.Add(acc);
                context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
            
        }
        public bool removeAccount(Account acc)
        {
            try
            {
                context.Remove(acc);
                context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
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
        public bool addPermision(string email, string permission)
        {
            try
            {
                user_permissions newPermission = new user_permissions(email, permission);
                context.permissions.Add(newPermission);
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }

    }
}
