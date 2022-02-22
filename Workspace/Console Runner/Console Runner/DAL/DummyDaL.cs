using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner.DAL
{
    public class DummyDaL : IDataAccess
    {

        List<Account> accounts = new();
        List<user_permissions> permissions = new();

        public bool accountExists(string email)
        {
            for(int i = 0; i < accounts.Count; i++)
            {
                if (email == accounts[i].Email){
                    return true;
                }
            }
            return false;
        }

        public bool addAccount(Account acc)
        {
            accounts.Add(acc);

            return true;
        }

        public bool addHistoryItem()
        {
            throw new NotImplementedException();
        }

        public bool addPermission(string email, string permission)
        {
            user_permissions newPermission = new user_permissions(email, permission,this);
            permissions.Add(newPermission);
            return true;
        }

        public int AdminCount()
        {
            int count = 0;
            for(int i = 0; i < accounts.Count(); i++)
            {
                if (hasPermission(accounts[i].Email, "createAdmin") && accounts[i].isActive)
                {
                    count++;
                }
            }
            return count;
        }

        public Account getAccount(string email)
        {
            for(int i = 0;i < accounts.Count;i++)
            {
                if(accounts[i].Email == email)
                {
                    return accounts[i];
                }
            }
            return null;
        }

        public List<user_permissions> getAllUserPermissions(string email)
        {
            List<user_permissions> usersPerms = new List<user_permissions>();
            for(int i = 0; i < permissions.Count;i++)
            {
                if(permissions[i].email == email)
                {
                    usersPerms.Add(permissions[i]);
                }
            }
            return usersPerms;
        }

        public bool hasPermission(string email, string permission)
        {
            for(int i = 0; i < permissions.Count ; i++)
            {
                if(permissions[i].email == email && permissions[i].permission == permission)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isAdmin(string email)
        {
            for (int i = 0; i < permissions.Count; i++)
            {
                if (permissions[i].email == email && permissions[i].permission == "createAdmin")
                {
                    return true;
                }
            }
            return false;
        }

        public bool removeAccount(Account acc)
        {
            for(int i = 0; i < accounts.Count;i++)
            {
                if(accounts[i].Email == acc.Email)
                {
                    accounts.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool removeAllUserPermissions(string email)
        {
            for(int i = 0; i <permissions.Count ; i++)
            {
                if (permissions[i].email == email)
                {
                    permissions.RemoveAt(i);
                    return true;
                }
            }
            return false ;
        }

        public bool removePermision(string email, string permission)
        {
            try
            {
                user_permissions perm = new user_permissions(email, permission,this);
                permissions.Remove(perm);
                return true;
            }catch (Exception e)
            {
                return false ;
            }
            
        }

        public bool updateAccount(Account acc)
        {
            for(int i = 0; i < accounts.Count ; i++)
            {
                if(accounts[i].Email == acc.Email)
                {
                    accounts[i] = acc;
                    return true;
                }
            }
            return false;
        }

    }
}
