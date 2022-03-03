using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner;
using Console_Runner.User_Management;

namespace Console_Runner.DAL
{
    public class EFPermissionGateway : IPermissionGateway
    {
        private readonly Context _efContext;

        public EFPermissionGateway(Context dbContext)
        {
            _efContext = dbContext;
        }

        public bool HasPermission(string email, string resource)
        {
            return _efContext.Permissions.Find(email, resource) != null;
        }

        public bool AddPermission(Permission permissionToAdd)
        {
            try
            {
                if (_efContext.Permissions.Find(permissionToAdd.Email, permissionToAdd.Resource) == null)
                {
                    _efContext.Permissions.Add(permissionToAdd);
                    _efContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddPermissions(List<Permission> permissionsToAdd)
        {
            try
            {
                foreach (var permission in permissionsToAdd)
                {
                    _efContext.Permissions.Add(permission);
                }
                _efContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemovePermission(string email, string resource)
        {
            try
            {
                Permission newPermission = new Permission(email, resource);
                if (HasPermission(email, resource))
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

        public List<Permission> GetAllUserPermissions(string email)
        {
            List<Permission> alluserPermissions = new List<Permission>();
            foreach (var permissions in _efContext.Permissions)
            {
                if (permissions.Email == email)
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
                if (permissions.Email == email)
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
