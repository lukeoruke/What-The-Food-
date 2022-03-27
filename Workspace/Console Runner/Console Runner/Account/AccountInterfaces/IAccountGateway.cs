

namespace Console_Runner.AccountService
{
    public interface IAccountGateway
    {
        /// <summary>
        /// Verify whether an Account object exists in the database with the provided ID.
        /// </summary>
        /// <param name="UserID">UserID to search for</param>
        /// <returns>True if the searched account exists, false otherwise.</returns>
        public Task<bool> AccountExistsAsync(int UserID);

        /// <summary>
        /// Retrieve an Account object from the database.
        /// </summary>
        /// <param name="UserID">UserID  to retrieve</param>
        /// <returns>Account object with the provided AccountID assuming it exists, otherwise null if the account does not exist.</returns>
        public Task<Account?> GetAccountAsync(int UserID);

        /// <summary>
        /// Add an Account object to the database.
        /// </summary>
        /// <param name="acc"> Account object to add to the database</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public Task<bool> AddAccountAsync(Account acc);

        /// <summary>
        /// Remove an Account object from the database.
        /// </summary>
        /// <param name="acc">The Account object being removed from the database</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public Task<bool> RemoveAccountAsync(Account acc);

        /// <summary>
        /// Update an Account object in the database. Modify the account object, then pass it into this method. The corresponding object in the database will be updated accordingly.
        /// </summary>
        /// <param name="acc">The Account object with modified parameters</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public Task<bool> UpdateAccountAsync(Account acc);

        public int GetIDFromEmail(string email);

        public int NumberOfAccounts();

        public string getSalt(int userID);

    }
}
