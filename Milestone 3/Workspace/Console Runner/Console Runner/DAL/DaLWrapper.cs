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

        public bool accountExists(string email)
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
                if(accountExists(acc.Email))
                {
                    context.Remove(acc);
                    context.SaveChanges();
                }
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }

        public bool updateAccount(Account acc)
        {
            try
            {
                context.accounts.Update(acc);
                context.SaveChanges(true);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
            
        }
        /////////////////////////////////////////////////////////////Permissions////////////////////////////////////////////////////////////////////////////////////////////////
        public bool hasPermission(string email, string permission)
        {
            return context.permissions.Find(email, permission) != null;
        }
        public bool addPermission(string email, string permission)
        {
            try
            {
                user_permissions newPermission = new user_permissions(email, permission);
                if (context.permissions.Find(email, permission) == null)
                {
                    context.permissions.Add(newPermission);
                    context.SaveChanges();
                }
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }
        public bool removePermision(string email, string permission)
        {
            try
            {
                user_permissions newPermission = new user_permissions(email, permission);
                if(hasPermission(email, permission))
                {
                    context.permissions.Remove(newPermission);
                    context.SaveChanges();
                    return true;
                }
            }catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public List<user_permissions>  getAllUserPermissions(string email)
        {
            List<user_permissions> alluserPermissions = new List<user_permissions>();
            foreach (var permissions in context.permissions)
            {
                if(permissions.email == email)
                {
                    alluserPermissions.Add(permissions);
                }
            }
            return alluserPermissions;
        }

        public bool removeAllUserPermissions(string email)
        {
            foreach (var permissions in context.permissions)
            {
                if (permissions.email == email)
                {
                    context.permissions.Remove(permissions);
                }
            }
            return true;
        }

        //returns the number of admins in the database
        public int AdminCount()
        {
            int count = 0;
            using (var context = new Context())
            {
                foreach (var account in context.accounts)
                {
                    if (hasPermission(account.Email, "createAdmin") && account.isActive) count++;
                }
            }
            return count;
        }

    }
}
