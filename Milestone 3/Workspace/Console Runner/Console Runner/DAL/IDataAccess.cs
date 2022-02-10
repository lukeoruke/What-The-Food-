using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner.DAL
{
    public interface IDataAccess
    {
        public bool accountExists(string email);
        public Account getAccount(string email);
        public bool addAccount(Account acc);
        public bool removeAccount(Account acc);
        public bool updateAccount(Account acc);
        public bool hasPermission(string email, string permission);
        public bool addPermission(string email, string permission);
        public bool removePermision(string email, string permission);
        public List<user_permissions> getAllUserPermissions(string email);
        public bool removeAllUserPermissions(string email);
        public int AdminCount();

        public bool isAdmin(string email);
    }
}
