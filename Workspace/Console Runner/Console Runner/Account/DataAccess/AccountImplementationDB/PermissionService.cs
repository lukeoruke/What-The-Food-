using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console_Runner.AccountService
{
    public class PermissionService
    {
        private readonly IAuthorizationGateway _permissionAccess;
        private readonly ImmutableList<String> DEFAULT_USER_PERMISSIONS;
        private readonly ImmutableList<String> ADMIN_PERMISSIONS;

        public PermissionService(IAuthorizationGateway permissionAccessor)
        {
            _permissionAccess = permissionAccessor;
            DEFAULT_USER_PERMISSIONS = ImmutableList.Create<String>(new String[] 
                {"scanFood", "editOwnAccount", "leaveReview", "deleteOwnAccount", "historyAccess", "AMR", "foodFlag" });
            ADMIN_PERMISSIONS = ImmutableList.Create<String>(new String[]
                {"enableAccount", "disableAccount", "deleteAccount", "createAdmin", "editOtherAccount"});
        }

        public bool HasPermission(string email, string resource)
        {
            return _permissionAccess.HasPermissionAsync(email, resource);
        }

        public int AdminCount()
        {
            return _permissionAccess.AdminCountAsync();
        }

        public bool IsAdmin(string email)
        {
            return _permissionAccess.IsAdminAsync(email);
        }

        public bool RemoveAllUserPermissions(string email)
        {
            return _permissionAccess.RemoveAllUserPermissionsAsync(email);
        }

        /* contains a package of the defualt permissions that will be assigned to all new user accounts.
        * Email: the PK of the account we are giving these permissions to
        */
        public void AssignDefaultUserPermissionsAsync(string email)
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
        public void AssignDefaultAdminPermissionsAsync(string email)
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
