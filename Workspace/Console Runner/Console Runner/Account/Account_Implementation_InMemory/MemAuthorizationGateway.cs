
using System.Collections.Immutable;

namespace Console_Runner.AccountService
{
    public class MemAuthorizationGateway : IAuthorizationGateway
    {
        readonly ImmutableList<string> DEFAULT_USER_PERMISSIONS;
        readonly ImmutableList<string> DEFAULT_ADMIN_PERMISSIONS;
        private List<Authorization> _memAuthContext;
        public MemAuthorizationGateway()
        {
            DEFAULT_USER_PERMISSIONS = ImmutableList.Create<string>(new string[]
            {"scanFood", "editOwnAccount", "leaveReview", "deleteOwnAccount", "historyAccess", "AMR", "foodFlag" });
            DEFAULT_ADMIN_PERMISSIONS = ImmutableList.Create<string>(new string[]
                {"enableAccount", "disableAccount", "deleteAccount", "createAdmin", "editOtherAccount"});
            _memAuthContext = new List<Authorization>(); 
        }
        public async Task<bool> AddPermissionAsync(Authorization permissionToAdd)
        {
            try
            {
                _memAuthContext.Add(permissionToAdd);
                 return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddPermissionsAsync(List<Authorization> permissionsToAdd)
        {
            try
            {
                foreach (Authorization permission in permissionsToAdd)
                {
                    await AddPermissionAsync(permission);
                }
                return true;
            } catch (Exception ex)
            {
                return false;
            }
            
        }

        public int AdminCount()
        {
            int count = 0;
            foreach(Authorization permission in _memAuthContext)
            {
                if(permission.Permission == "createAdmin")
                {
                    count++;
                }
            }
            return count;
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
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Authorization> GetAllUserPermissions(int userID)
        {
            List<Authorization> permissions = new List<Authorization>();
            var alluserPermissions = _memAuthContext.Where(r => r.UserID == userID);
            foreach (Authorization perms in alluserPermissions)
            {
                permissions.Add(perms);
            }
            return permissions;
        }

        public Task<bool> HasPermissionAsync(int userID, string permission)
        {
            throw new NotImplementedException();
        }

        public bool IsAdmin(int userID)
        {
            foreach(Authorization permission in _memAuthContext)
            {
                if(permission.UserID == userID && permission.Permission == "createAdmin")
                {
                    return true;
                }
                
            }
            return false;
        }

        public bool RemoveAllUserPermissions(int userID)
        {
            try
            {
                foreach (Authorization permission in _memAuthContext)
                {
                    if (permission.UserID == userID)
                    {
                        _memAuthContext.Remove(permission);
                    }
                }
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }

        public Task<bool> RemovePermissionsAsync(int userID, string permissions)
        {
            try
            {
                _memAuthContext.Remove(new Authorization(userID, permissions));
            }
            catch(Exception ex)
            {

            }
        }
    }
}
