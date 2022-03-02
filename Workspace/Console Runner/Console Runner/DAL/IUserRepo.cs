using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner.DAL
{
    public interface IUserRepo
    {
        public bool AccountExists(string email);
        public Account GetAccount(string email);
        public bool AddAccount(Account acc);
        public bool RemoveAccount(Account acc);
        public bool UpdateAccount(Account acc);
        public bool HasPermission(string email, string permission);
        public bool AddPermission(string email, string permission);
        public bool RemovePermision(string email, string permission);
        public List<user_permissions> GetAllUserPermissions(string email);
        public int AdminCount();
        public bool AddHistoryItem();
        public bool IsAdmin(string email);
    }
}
