


namespace Console_Runner.AccountService
{
    public interface IAuthorizationGateway
    {
        /// <summary>
        /// Checks if the specified Account has a specified Permission associated with it.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <param name="Permission">The associated Permission to check for</param>
        /// <returns>True if the Account has the specified Permission, false otherwise.</returns>
        public Task<bool> HasPermissionAsync(int userID, string permission);

        /// <summary>
        /// Adds a Permission to the database.
        /// </summary>
        /// <param name="PermissionsToAdd">The Permission to add to the database</param>
        /// <returns>True if the Permission was successfully added to the database, false otherwise.</returns>
        public Task<bool> AddPermissionAsync(Authorization permissionToAdd);
        /// <summary>
        /// Adds a set of Permissions to the database.
        /// </summary>
        /// <param name="permissionsToAdd">The List of Permissions to add to the database</param>
        /// <returns>True if the Permissions in the List were successfully added to the database, false otherwise.</returns>
        public Task<bool> AddPermissionsAsync(List<Authorization> permissionsToAdd);

        /// <summary>
        /// Removes a specified Permission from the specified Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <param name="permissions">The associated permissions to remove</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public Task<bool> RemovePermissionsAsync(int userID, string permissions);

        /// <summary>
        /// Gets a list of all Permissions associated with an Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>A List containing all User_Permissions associated with the specified Account.</returns>
        public List<Authorization> GetAllUserPermissions(int userID);

        /// <summary>
        /// Removes all Permissions associated with a specified Account.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool RemoveAllUserPermissions(int userID);
        
        /// <summary>
        /// Counts the number of Accounts on the database currently assigned as administrators.
        /// </summary>
        /// <returns>An int reflecting the number of Accounts on the database with administrator permissions.</returns>
        public int AdminCount();

        /// <summary>
        /// Determines if a specified Account is assigned as an administrator in the database.
        /// </summary>
        /// <param name="userID">The specified Account's userID</param>
        /// <returns>True if the specified Account is an administrator, false otherwise.</returns>
        public bool IsAdmin(int userID);

        public Task<bool> AssignDefaultUserPermissions(int userID);
        public Task<bool> AssignDefaultAdminPermissions(int userID);

    }
}
