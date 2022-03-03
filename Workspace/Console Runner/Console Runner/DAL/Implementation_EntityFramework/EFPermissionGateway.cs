using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner;

namespace Console_Runner.DAL
{
    public class EFPermissionGateway : IPermissionGateway
    {
        private readonly Context _efContext;

        public EFPermissionGateway(Context dbContext)
        {
            _efContext = dbContext;
        }

        public bool HasPermission(string email, string permission)
        {
            return _efContext.Permissions.Find(email, permission) != null;
        }
        public bool AddPermission(string email, string permission)
        {
            try
            {
                user_permissions newPermission = new user_permissions(email, permission, this);
                if (_efContext.Permissions.Find(email, permission) == null)
                {
                    _efContext.Permissions.Add(newPermission);
                    _efContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool RemovePermission(string email, string permission)
        {
            try
            {
                user_permissions newPermission = new user_permissions(email, permission, this);
                if (HasPermission(email, permission))
                {
                    _efContext.Permissions.Remove(newPermission);
                    _efContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public List<user_permissions> GetAllUserPermissions(string email)
        {
            List<user_permissions> alluserPermissions = new List<user_permissions>();
            foreach (var permissions in _efContext.Permissions)
            {
                if (permissions.email == email)
                {
                    alluserPermissions.Add(permissions);
                }
            }
            return alluserPermissions;
        }

        public bool RemoveAllUserPermissions(string email)
        {
            foreach (var permissions in _efContext.Permissions)
            {
                if (permissions.email == email)
                {
                    _efContext.Permissions.Remove(permissions);
                }
            }
            return true;
        }

        public int AdminCount()
        {
            int count = 0;
            using (var context = new Context())
            {
                foreach (var account in context.Accounts)
                {
                    if (HasPermission(account.Email, "createAdmin") && account.isActive) count++;
                }
            }
            return count;
        }

        public bool IsAdmin(string email)
        {
            using (var context = new Context())
            {
                foreach (var account in context.Accounts)
                {
                    if (HasPermission(account.Email, "createAdmin") && account.isActive)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
