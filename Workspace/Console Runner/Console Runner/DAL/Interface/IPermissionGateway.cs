using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.User_Management;

namespace Console_Runner.DAL
{
    public interface IPermissionGateway
    {
        /// <summary>
        /// Checks if the specified Account has a specified Permission associated with it.
        /// </summary>
        /// <param name="email">The specified Account's email</param>
        /// <param name="resource">The associated Permission to check for</param>
        /// <returns>True if the Account has the specified Permission, false otherwise.</returns>
        public bool HasPermission(string email, string resource);
        
        /// <summary>
        /// Adds a Permission to the database.
        /// </summary>
        /// <param name="permissionToAdd">The Permission to add to the database</param>
        /// <returns>True if the Permission was successfully added to the database, false otherwise.</returns>
        public bool AddPermission(Permission permissionToAdd);


        /// <summary>
        /// Adds a set of Permissions to the database.
        /// </summary>
        /// <param name="permissionsToAdd">The List of Permissions to add to the database</param>
        /// <returns>True if the Permissions in the List were successfully added to the database, false otherwise.</returns>
        public bool AddPermissions(List<Permission> permissionsToAdd);

        /// <summary>
        /// Removes a specified Permission from the specified Account.
        /// </summary>
        /// <param name="email">The specified Account's email</param>
        /// <param name="permission">The associated Permission to remove</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool RemovePermission(string email, string resource);

        /// <summary>
        /// Gets a list of all Permissions associated with an Account.
        /// </summary>
        /// <param name="email">The specified Account's email</param>
        /// <returns>A List containing all User_Permissions associated with the specified Account.</returns>
        public List<Permission> GetAllUserPermissions(string email);

        /// <summary>
        /// Removes all Permissions associated with a specified Account.
        /// </summary>
        /// <param name="email">The specified Account's email</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool RemoveAllUserPermissions(string email);
        
        /// <summary>
        /// Counts the number of Accounts on the database currently assigned as administrators.
        /// </summary>
        /// <returns>An int reflecting the number of Accounts on the database with administrator permissions.</returns>
        public int AdminCount();

        /// <summary>
        /// Determines if a specified Account is assigned as an administrator in the database.
        /// </summary>
        /// <param name="email">The specified Account's email</param>
        /// <returns>True if the specified Account is an administrator, false otherwise.</returns>
        public bool IsAdmin(string email);
    }
}
