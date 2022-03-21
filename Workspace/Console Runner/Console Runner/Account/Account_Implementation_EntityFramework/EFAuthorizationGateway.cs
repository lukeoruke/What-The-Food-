using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner;
using Console_Runner.FoodService;

namespace Console_Runner.AccountService
{
    public class EFAuthorizationGateway : IAuthorizationGateway
    {
        private readonly ContextAccountDB _efContext;
        private readonly ImmutableList<string> DEFAULT_USER_PERMISSIONS;
        private readonly ImmutableList<string> DEFAULT_ADMIN_PERMISSIONS;
        public EFAuthorizationGateway()
        {
            DEFAULT_USER_PERMISSIONS = ImmutableList.Create<string>(new string[]
            {"scanFood", "editOwnAccount", "leaveReview", "deleteOwnAccount", "historyAccess", "AMR", "foodFlag" });
            DEFAULT_ADMIN_PERMISSIONS = ImmutableList.Create<string>(new string[]
                {"enableAccount", "disableAccount", "deleteAccount", "createAdmin", "editOtherAccount"});
            _efContext = new ContextAccountDB();
        }

        public async Task<bool> HasPermissionAsync(int userID, string permission)
        {
            return await _efContext.Authorizations.FindAsync(userID, permission) != null;
        }

        public async Task<bool> AddPermissionAsync(Authorization permissionToAdd)
        {
            try
            {
                if (await _efContext.Authorizations.FindAsync(permissionToAdd.UserID, permissionToAdd.Permission) == null)
                {
                    await _efContext.Authorizations.AddAsync(permissionToAdd);
                    await _efContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddPermissionsAsync(List<Authorization> permissionsToAdd)
        {
            try
            {
                foreach (var permission in permissionsToAdd)
                {
                    await _efContext.Authorizations.AddAsync(permission);
                }
                await _efContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemovePermissionsAsync(int userID, string permissions)
        {
            try
            {
                Authorization newPermission = new Authorization(userID, permissions);
                if (await HasPermissionAsync(userID, permissions))
                {
                    _efContext.Authorizations.Remove(newPermission);
                    await _efContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public List<Authorization> GetAllUserPermissions(int userID)
        {
            List<Authorization> permissions = new List<Authorization>();
            var alluserPermissions = _efContext.Authorizations.Where(r => r.UserID == userID);
            foreach (Authorization perms in alluserPermissions)
            {
                permissions.Add(perms);
            }
            return permissions;
        }

        public bool RemoveAllUserPermissions(int userID)
        {
            foreach (var permissions in _efContext.Authorizations.Where(r => r.UserID == userID))
            {
                if (permissions.UserID == userID)
                {
                    _efContext.Authorizations.Remove(permissions);
                }
            }
            return true;
        }

        public int AdminCount()
        {
            var adminList = _efContext.Authorizations.Where(r => r.Permission == "createAdmin");
            return adminList.Count();
        }

        public bool IsAdmin(int userID)
        {
            var adminList = _efContext.Authorizations.Where(r => r.Permission == "createAdmin");
            foreach(var admin in adminList)
            {
                if (admin.UserID == userID)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> AssignDefaultUserPermissions(int userID)
        {
            try
            {
                List<Authorization> defaultPermsToAdd = new();
                foreach (String permission in DEFAULT_USER_PERMISSIONS)
                {
                    defaultPermsToAdd.Add(new Authorization(userID, permission));
                }
                await AddPermissionsAsync(defaultPermsToAdd);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AssignDefaultAdminPermissions(int userID)
        {
            try
            {
                List<Authorization> defaultPermsToAdd = new();
                foreach (String permission in DEFAULT_ADMIN_PERMISSIONS)
                {
                    defaultPermsToAdd.Add(new Authorization(userID, permission));
                }
                await AddPermissionsAsync(defaultPermsToAdd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
