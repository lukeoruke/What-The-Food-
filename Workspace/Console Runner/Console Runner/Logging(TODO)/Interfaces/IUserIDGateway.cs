using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Logging
{
    public interface IUserIDGateway
    {
        /// <summary>
        /// Gets the hash corresponding to the given user ID.
        /// </summary>
        /// <param name="idToGet">The user ID to get the corresponding hash of.</param> 
        /// <returns>The string representation of the hashed user ID if it is on the database, null otherwise.</returns>
        public Task<string?> GetUserHashAsync(string idToGet, CancellationToken cancellationToken);
        /// <summary>
        /// Adds the given user ID to the database and generates its corresponding hash, if it does not already exist, and returns the corresponding hash.
        /// </summary>
        /// <param name="idToAdd">The user ID to add to the database</param>
        /// <returns>The hash corresponding to the user ID to add.</returns>
        public Task<string> AddUserIdAsync(string idToAdd, CancellationToken cancellationToken);
        /// <summary>
        /// Removes the given user ID and its corresponding hash from the database, if it exists on the database.
        /// </summary>
        /// <param name="idToRemove">The user ID to remove from the database with its corresponding hash.</param>
        /// <returns>True if the ID was successfully removed from the database.</returns>
        public Task<bool> RemoveUserIdAsync(string idToRemove, CancellationToken cancellationToken);
    }
}
