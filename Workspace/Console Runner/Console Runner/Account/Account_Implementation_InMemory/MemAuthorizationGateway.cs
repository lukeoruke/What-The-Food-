using System.Collections.Immutable;

using Console_Runner.Logging;

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
        public async Task<bool> AddPermissionAsync(Authorization permissionToAdd, LogService? logService = null)
        {
            try
            {
                _memAuthContext.Add(permissionToAdd);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                        $"Created authorization for user {permissionToAdd.UserID} and resource {permissionToAdd.Permission}");
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
                foreach (Authorization permission in permissionsToAdd)
                {
                    _memAuthContext.Add(permission);
                    dbActionLogs.Add(new LogData()
                    {
                        LogLevel = LogLevel.Info,
                        Category = Category.DataStore,
                        Timestamp = DateTime.Now,
                        Message = $"Created authorization for user {permission.UserID} and resource {permission.Permission}"
                    });
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
            int count = 0;
            foreach(Authorization permission in _memAuthContext)
            {
                if(permission.Permission == "createAdmin")
                {
                    count++;
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved admin count");
            }
            return count;
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Authorization> GetAllUserPermissions(int userID, LogService? logService = null)
        {
            List<Authorization> permissions = new List<Authorization>();
            var alluserPermissions = _memAuthContext.Where(r => r.UserID == userID);
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

        public async Task<bool> HasPermissionAsync(int userID, string permission, LogService? logService = null)
        {
            foreach(Authorization perm in _memAuthContext)
            {
                if(perm.UserID == userID && perm.Permission == permission)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                            $"Retrieved authorization for user {userID} and resource {permission}");
                    }
                    return true;
                }
            }
            return false;
        }

        public bool IsAdmin(int userID, LogService? logService = null)
        {
            foreach(Authorization permission in _memAuthContext)
            {
                if(permission.UserID == userID && permission.Permission == "createAdmin")
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

        public bool RemoveAllUserPermissions(int userID, LogService? logService = null)
        {
            try
            {
                List<LogData> dbActionLogs = new();
                foreach (Authorization permission in _memAuthContext)
                {
                    if (permission.UserID == userID)
                    {
                        _memAuthContext.Remove(permission);
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

        public async Task<bool> RemovePermissionsAsync(int userID, string permission, LogService? logService = null)
        {
            try
            {
                Authorization temp = new Authorization(permission);
                temp.UserID = userID;
                _memAuthContext.Remove(temp);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Removed authorization for user {userID} and resource {permissions}");
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
