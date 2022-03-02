using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class1;
using Console_Runner;

namespace Console_Runner.DAL
{
    public class EFPermissionRepo : IPermissionRepo
    {
        private Context EFContext = new();

        public bool HasPermission(string email, string permission)
        {
            return EFContext.Permissions.Find(email, permission) != null;
        }
        public bool AddPermission(string email, string permission)
        {
            try
            {
                user_permissions newPermission = new user_permissions(email, permission, this);
                if (EFContext.Permissions.Find(email, permission) == null)
                {
                    EFContext.Permissions.Add(newPermission);
                    EFContext.SaveChanges();
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
                    EFContext.Permissions.Remove(newPermission);
                    EFContext.SaveChanges();
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
            foreach (var permissions in EFContext.Permissions)
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
            foreach (var permissions in EFContext.Permissions)
            {
                if (permissions.email == email)
                {
                    EFContext.Permissions.Remove(permissions);
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
