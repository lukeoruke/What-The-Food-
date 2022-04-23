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

        /// <summary>
        /// Checks if the specified Account has a specified Permission associated with it.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <param name="Permission">The associated Permission to check for</param>
        /// <returns>True if the Account has the specified Permission, false otherwise.</returns>
        public async Task<bool> HasPermissionAsync(int userID, string permission, LogService? logService = null)
        {
            Authorization? perm = await _efContext.Authorizations.FindAsync(userID, permission);
            bool toReturn = perm != null;
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogSeverity.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved authorization for user {userID} and resource {permission}");
            }
            return toReturn;
        }
        /// <summary>
        /// Adds a Permission to the database.
        /// </summary>
        /// <param name="PermissionsToAdd">The Permission to add to the database</param>
        /// <returns>True if the Permission was successfully added to the database, false otherwise.</returns>
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
                        _ = logService.LogWithSetUserAsync(LogSeverity.Info, Category.DataStore, DateTime.Now,
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
        /// <summary>
        /// Adds a set of Permissions to the database.
        /// </summary>
        /// <param name="permissionsToAdd">The List of Permissions to add to the database</param>
        /// <returns>True if the Permissions in the List were successfully added to the database, false otherwise.</returns>
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
                        LogLevel = LogSeverity.Info,
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
        /// <summary>
        /// Removes a specified Permission from the specified Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <param name="permissions">The associated permissions to remove</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
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
                        _ = logService.LogWithSetUserAsync(LogSeverity.Info, Category.DataStore, DateTime.Now,
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
        /// <summary>
        /// Gets a list of all Permissions associated with an Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>A List containing all User_Permissions associated with the specified Account.</returns>
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
                _ = logService.LogWithSetUserAsync(LogSeverity.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved all authorizations for user {userID}");
            }
            return permissions;
        }
        /// <summary>
        /// Removes all Permissions associated with a specified Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
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
                            LogLevel = LogSeverity.Info,
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
        /// <summary>
        /// Counts the number of Accounts on the database currently assigned as administrators.
        /// </summary>
        /// <returns>An int reflecting the number of Accounts on the database with administrator permissions.</returns>
        public int AdminCount(LogService? logService = null)
        {
            var adminList = _efContext.Authorizations.Where(r => r.Permission == "createAdmin");
            int adminCount = adminList.Count();
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogSeverity.Info, Category.DataStore, DateTime.Now,
                    $"Retrieved admin count");
            }
            return adminCount;
        }
        /// <summary>
        /// Determines if a specified Account is assigned as an administrator in the database.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>True if the specified Account is an administrator, false otherwise.</returns>
        public bool IsAdmin(int userID, LogService? logService = null)
        {
            var adminList = _efContext.Authorizations.Where(r => r.Permission == "createAdmin");
            foreach(var admin in adminList)
            {
                if (admin.UserID == userID)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogSeverity.Info, Category.DataStore, DateTime.Now,
                            $"Retrieved whether user {userID} is admin (true)");
                    }
                    return true;
                }
            }
            if (logService?.UserID != null)
            {
                _ = logService.LogWithSetUserAsync(LogSeverity.Debug, Category.DataStore, DateTime.Now,
                    $"Retrieved all authorizations for user {userID} (false)");
            }
            return false;
        }/// <summary>
         /// Assigns the default permissions every account has
         /// {"scanFood", "editOwnAccount", "leaveReview", "deleteOwnAccount", "historyAccess", "AMR", "foodFlag" });
         /// </summary>
         /// <param name="userID"></param>
         /// <param name="logService"></param>
         /// <returns></returns>
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
                    _ = logService.LogWithSetUserAsync(LogSeverity.Info, Category.DataStore, DateTime.Now,
                        $"Created default user permissions for user {userID}");
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Assigns the default permissions every admin account has
        /// {"enableAccount", "disableAccount", "deleteAccount", "createAdmin", "editOtherAccount"});
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns></returns>
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
                    _ = logService.LogWithSetUserAsync(LogSeverity.Info, Category.DataStore, DateTime.Now,
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
