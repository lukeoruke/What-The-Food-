using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public interface IAccountGateway
    {
        /// <summary>
        /// Verify whether an Account object exists in the database with the provided ID.
        /// </summary>
        /// <param name="UserID">UserID to search for</param>
        /// <returns>True if the searched account exists, false otherwise.</returns>
        public Task<bool> AccountExistsAsync(int UserID, LogService? logService = null);

        /// <summary>
        /// Retrieve an Account object from the database.
        /// </summary>
        /// <param name="UserID">UserID  to retrieve</param>
        /// <returns>Account object with the provided AccountID assuming it exists, otherwise null if the account does not exist.</returns>
        public Task<Account?> GetAccountAsync(int UserID, LogService? logService = null);

        /// <summary>
        /// Add an Account object to the database.
        /// </summary>
        /// <param name="acc"> Account object to add to the database</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public Task<bool> AddAccountAsync(Account acc, LogService? logService = null);

        /// <summary>
        /// Remove an Account object from the database.
        /// </summary>
        /// <param name="acc">The Account object being removed from the database</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public Task<bool> RemoveAccountAsync(Account acc, LogService? logService = null);

        /// <summary>
        /// Update an Account object in the database. Modify the account object, then pass it into this method. The corresponding object in the database will be updated accordingly.
        /// </summary>
        /// <param name="acc">The Account object with modified parameters</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public Task<bool> UpdateAccountAsync(Account acc, LogService? logService = null);
        /// <summary>
        /// Gets a users ID from their email address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="logService"></param>
        /// <returns>the ID associated with a specifici email</returns>
        public Task<int> GetIDFromEmailIdAsync(string email, LogService? logService = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logService"></param>
        /// <returns>returns the number of accounts</returns>
        public int NumberOfAccounts(LogService? logService = null);
        /// <summary>
        /// Gets the salt associated with a user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="logService"></param>
        /// <returns>The salt associated with a user</returns>
        public string getSalt(int userID, LogService? logService = null);

        public  Task<bool> ToggleDataCollectionStatus(int userID);
    }
}
