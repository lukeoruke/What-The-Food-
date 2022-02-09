using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner.DAL
{
    internal class DummyDaL : IDataAccess
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
            throw new NotImplementedException();
        }

        public bool addAccount(Account acc)
        {
            accounts.Add(acc);
            throw new NotImplementedException();
        }

        public bool addPermission(string email, string permission)
        {
            user_permissions newPermission = new user_permissions(email, permission);
            permissions.Add(newPermission);
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool removeAccount(Account acc)
        {
            accounts.Remove(acc);
            throw new NotImplementedException();
        }

        public bool removeAllUserPermissions(string email)
        {
            for(int i = 0; i <permissions.Count ; i++)
            {
                if (permissions[i].email == email)
                {
                    permissions.RemoveAt(i);
                }
            }
            throw new NotImplementedException();
        }

        public bool removePermision(string email, string permission)
        {
            user_permissions perm = new user_permissions(email,permission);
            permissions.Remove(perm);
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
