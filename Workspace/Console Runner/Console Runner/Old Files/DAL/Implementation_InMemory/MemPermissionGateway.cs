using Console_Runner.User_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.User_Management;

namespace Console_Runner.DAL
{
    public class MemAuthorizationGateway : IPermissionGateway
    {
        private List<Permission> _permissionDB;
        
        public MemAuthorizationGateway()
        {
            _permissionDB = new List<Permission>();
        }
        public bool AddPermission(Permission permissionToAdd)
        {
            _permissionDB.Add(permissionToAdd);
            return true;
        }

        public bool AddPermissions(List<Permission> permissionsToAdd)
        {
            foreach (Permission permissionToAdd in permissionsToAdd)
            {
                _permissionDB.Add(permissionToAdd);
            }
            return true;
        }

        public int AdminCount()
        {
            return _permissionDB.FindAll(perm => perm.Resource == "createAdmin").Count;
        }

        public List<Permission> GetAllUserPermissions(string email)
        {
            return _permissionDB.FindAll(perm => perm.Email == email);
        }

        public bool HasPermission(string email, string resource)
        {
            return _permissionDB.FindIndex(perm => perm.Email == email && perm.Resource == resource) != -1;
        }

        public bool IsAdmin(string email)
        {
            return HasPermission(email, "createAdmin");
        }

        public bool RemoveAllUserPermissions(string email)
        {
            _permissionDB.RemoveAll(perm => perm.Email == email);
            return true;
        }

        public bool RemovePermission(string email, string resource)
        {
            _permissionDB.RemoveAll(perm =>perm.Email == email && perm.Resource == resource);
            return true;
        }
    }
}
