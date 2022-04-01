using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.DAL;

namespace Console_Runner.User_Management
{
    public class PermissionService
    {
        private readonly IPermissionGateway _permissionAccess;
        private readonly ImmutableList<String> DEFAULT_USER_PERMISSIONS;
        private readonly ImmutableList<String> ADMIN_PERMISSIONS;

        public PermissionService(IPermissionGateway permissionAccessor)
        {
            _permissionAccess = permissionAccessor;
            DEFAULT_USER_PERMISSIONS = ImmutableList.Create<String>(new String[] 
                {"scanFood", "editOwnAccount", "leaveReview", "deleteOwnAccount", "historyAccess", "AMR", "foodFlag" });
            ADMIN_PERMISSIONS = ImmutableList.Create<String>(new String[]
                {"enableAccount", "disableAccount", "deleteAccount", "createAdmin", "editOtherAccount"});
        }

        public bool HasPermission(string email, string resource)
        {
            return _permissionAccess.HasPermission(email, resource);
        }

        public int AdminCount()
        {
            return _permissionAccess.AdminCount();
        }

        public bool IsAdmin(string email)
        {
            return _permissionAccess.IsAdmin(email);
        }

        public bool RemoveAllUserPermissions(string email)
        {
            return _permissionAccess.RemoveAllUserPermissions(email);
        }

        /* contains a package of the defualt permissions that will be assigned to all new user accounts.
        * Email: the PK of the account we are giving these permissions to
        */
        public void AssignDefaultUserPermissions(string email)
        {
            List<Permission> defaultPermsToAdd = new();
            foreach(String resource in DEFAULT_USER_PERMISSIONS)
            {
                defaultPermsToAdd.Add(new Permission(email, resource));
            }
            _permissionAccess.AddPermissions(defaultPermsToAdd);
        }
        /* contains a package of the defualt permissions that will be assigned to all new admin accounts.
         * Email: the PK of the account we are giving these permissions to
         */
        public void AssignDefaultAdminPermissions(string email)
        {
            List<Permission> defaultPermsToAdd = new();
            foreach (String resource in ADMIN_PERMISSIONS)
            {
                defaultPermsToAdd.Add(new Permission(email, resource));
            }
            _permissionAccess.AddPermissions(defaultPermsToAdd);
        }
    }
}
