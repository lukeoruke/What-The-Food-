using Console_Runner.Logging;


namespace Console_Runner.AccountService
{
/*    public enum ENUM_DEFAULT_USER_PERMISSIONS
    {
        scanFood,
        editOwnAccount,
        leaveReview,
        deleteOwnAccount,
        historyAccess,
        AMR,
        foodFlag
    }
    public enum ENUM_DEFAULT_ADMIN_PERMISSIONS
    {
        enableAccount,
        disableAccount,
        deleteAccount,
        createAdmin,
        editOtherAccount
    }*/
    public interface IAuthorizationGateway
    {
        /// <summary>
        /// Checks if the specified Account has a specified Permission associated with it.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <param name="Permission">The associated Permission to check for</param>
        /// <returns>True if the Account has the specified Permission, false otherwise.</returns>
        public Task<bool> HasPermissionAsync(int userID, string permission, LogService? logService = null);

        /// <summary>
        /// Adds a Permission to the database.
        /// </summary>
        /// <param name="PermissionsToAdd">The Permission to add to the database</param>
        /// <returns>True if the Permission was successfully added to the database, false otherwise.</returns>
        public Task<bool> AddPermissionAsync(Authorization permissionToAdd, LogService? logService = null);
        /// <summary>
        /// Adds a set of Permissions to the database.
        /// </summary>
        /// <param name="permissionsToAdd">The List of Permissions to add to the database</param>
        /// <returns>True if the Permissions in the List were successfully added to the database, false otherwise.</returns>
        public Task<bool> AddPermissionsAsync(List<Authorization> permissionsToAdd, LogService? logService = null);

        /// <summary>
        /// Removes a specified Permission from the specified Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <param name="permissions">The associated permissions to remove</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public Task<bool> RemovePermissionsAsync(int userID, string permissions, LogService? logService = null);

        /// <summary>
        /// Gets a list of all Permissions associated with an Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>A List containing all User_Permissions associated with the specified Account.</returns>
        public List<Authorization> GetAllUserPermissions(int userID, LogService? logService = null);

        /// <summary>
        /// Removes all Permissions associated with a specified Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool RemoveAllUserPermissions(int userID, LogService? logService = null);
        
        /// <summary>
        /// Counts the number of Accounts on the database currently assigned as administrators.
        /// </summary>
        /// <returns>An int reflecting the number of Accounts on the database with administrator permissions.</returns>
        public int AdminCount(LogService? logService = null);

        /// <summary>
        /// Determines if a specified Account is assigned as an administrator in the database.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>True if the specified Account is an administrator, false otherwise.</returns>
        public bool IsAdmin(int userID, LogService? logService = null);

        /// <summary>
        /// Assigns the default permissions every account has
        /// {"scanFood", "editOwnAccount", "leaveReview", "deleteOwnAccount", "historyAccess", "AMR", "foodFlag" });
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns></returns>
        public Task<bool> AssignDefaultUserPermissions(int userID, LogService? logService = null);
        /// <summary>
        /// Assigns the default permissions every admin account has
        /// {"enableAccount", "disableAccount", "deleteAccount", "createAdmin", "editOtherAccount"});
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns></returns>
        public Task<bool> AssignDefaultAdminPermissions(int userID, LogService? logService = null);

    }
}
