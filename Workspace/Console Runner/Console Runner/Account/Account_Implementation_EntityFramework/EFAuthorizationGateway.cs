using System.Collections.Immutable;

using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public class EFAuthorizationGateway : IAuthorizationGateway
    {
        private readonly ContextAccountDB _efContext;
        readonly ImmutableList<string> DEFAULT_USER_PERMISSIONS;
        readonly ImmutableList<string> DEFAULT_ADMIN_PERMISSIONS;
        public EFAuthorizationGateway()
        {
            //TODO MAKE THIS NOT HARD CODED.
            DEFAULT_USER_PERMISSIONS = ImmutableList.Create<string>(new string[]
            {"scanFood", "editOwnAccount", "leaveReview", "deleteOwnAccount", "historyAccess", "AMR", "foodFlag" });
            DEFAULT_ADMIN_PERMISSIONS = ImmutableList.Create<string>(new string[]
                {"enableAccount", "disableAccount", "deleteAccount", "createAdmin", "editOtherAccount"});
            _efContext = new ContextAccountDB();
        }

        public async Task<bool> HasPermissionAsync(int userID, string permission, LogService? logService = null)
        {
            Authorization? perm = await _efContext.Authorizations.FindAsync(userID, permission);
            bool toReturn = perm != null;
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved authorization for user {userID} and resource {permission}");
            }
            return toReturn;
        }

        public async Task<bool> AddPermissionAsync(Authorization permissionToAdd, LogService? logService = null)
        {
            try
            {
                if (await _efContext.Authorizations.FindAsync(permissionToAdd.UserID, permissionToAdd.Permission) == null)
                {
                    await _efContext.Authorizations.AddAsync(permissionToAdd);
                    await _efContext.SaveChangesAsync();
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Created authorization for user {permissionToAdd.UserID} and resource {permissionToAdd.Permission}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddPermissionsAsync(List<Authorization> permissionsToAdd, LogService? logService = null)
        {
            try
            {
                List<LogData> dbActionLogs = new();
                foreach (var permission in permissionsToAdd)
                {
                    await _efContext.Authorizations.AddAsync(permission);
                    dbActionLogs.Add(new LogData()
                    {
                        LogLevel = LogLevel.Info,
                        Category = Category.DataStore,
                        Timestamp = DateTime.Now,
                        Message = $"Created authorization for user {permission.UserID} and resource {permission.Permission}"
                    });
                }
                await _efContext.SaveChangesAsync();
                if (logService?.UserID != null)
                {
                    _ = logService.LogListWithSetUserAsync(dbActionLogs);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemovePermissionsAsync(int userID, string permissions, LogService? logService = null)
        {
            try
            {
                Authorization newPermission = new Authorization(permissions);
                newPermission.UserID = userID;
                if (await HasPermissionAsync(userID, permissions))
                {
                    _efContext.Authorizations.Remove(newPermission);
                    await _efContext.SaveChangesAsync();
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Removed authorization for user {userID} and resource {permissions}");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public List<Authorization> GetAllUserPermissions(int userID, LogService? logService = null)
        {
            List<Authorization> permissions = new List<Authorization>();
            var alluserPermissions = _efContext.Authorizations.Where(r => r.UserID == userID);
            foreach (Authorization perms in alluserPermissions)
            {
                permissions.Add(perms);
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved all authorizations for user {userID}");
            }
            return permissions;
        }

        public bool RemoveAllUserPermissions(int userID, LogService? logService = null)
        {
            try
            {
                List<LogData> dbActionLogs = new();
                foreach (var permissions in _efContext.Authorizations.Where(r => r.UserID == userID))
                {
                    if (permissions.UserID == userID)
                    {
                        _efContext.Authorizations.Remove(permissions);
                        dbActionLogs.Add(new LogData
                        {
                            LogLevel = LogLevel.Info,
                            Category = Category.DataStore,
                            Timestamp = DateTime.Now,
                            Message = $"Removed authorization for user {permissions.UserID} and resource {permissions.Permission}"
                        });
                    }
                }
                if (logService?.UserID != null)
                {
                    _ = logService.LogListWithSetUserAsync(dbActionLogs);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public int AdminCount(LogService? logService = null)
        {
            var adminList = _efContext.Authorizations.Where(r => r.Permission == "createAdmin");
            int adminCount = adminList.Count();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved admin count");
            }
            return adminCount;
        }

        public bool IsAdmin(int userID, LogService? logService = null)
        {
            var adminList = _efContext.Authorizations.Where(r => r.Permission == "createAdmin");
            foreach(var admin in adminList)
            {
                if (admin.UserID == userID)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Retrieved whether user {userID} is admin (true)");
                    }
                    return true;
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Debug, Category.DataStore, DateTime.Now,
                    $"Retrieved all authorizations for user {userID} (false)");
            }
            return false;
        }
        public async Task<bool> AssignDefaultUserPermissions(int userID, LogService? logService = null)
        {
            try
            {
                List<Authorization> defaultPermsToAdd = new();
                foreach (String permission in DEFAULT_USER_PERMISSIONS)
                {
                    Authorization temp = new Authorization(permission);
                    temp.UserID = userID;
                    defaultPermsToAdd.Add(temp);
                }
                await AddPermissionsAsync(defaultPermsToAdd);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created default user permissions for user {userID}");
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AssignDefaultAdminPermissions(int userID, LogService? logService = null)
        {
            try
            {
                List<Authorization> defaultPermsToAdd = new();
                foreach (String permission in DEFAULT_ADMIN_PERMISSIONS)
                {
                    Authorization temp = new Authorization(permission);
                    temp.UserID = userID;
                    defaultPermsToAdd.Add(temp);
                }
                await AddPermissionsAsync(defaultPermsToAdd);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created default admin permissions for user {userID}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
