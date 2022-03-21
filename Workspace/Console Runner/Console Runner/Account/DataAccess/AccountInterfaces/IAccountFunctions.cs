using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Account
{
    public interface IAccountFunctions
    {
        /// <summary>
        /// Verify whether an Account object exists in the database with the provided ID.
        /// </summary>
        /// <param name="AccountID">Account ID to search for</param>
        /// <returns>True if the searched account exists, false otherwise.</returns>
        public bool AccountExists(int AccountID);

        /// <summary>
        /// Retrieve an Account object from the database.
        /// </summary>
        /// <param name="AccountID">Account ID to retrieve</param>
        /// <returns>Account object with the provided AccountID assuming it exists, otherwise null if the account does not exist.</returns>
        public Account? GetAccount(int AccountID);

        /// <summary>
        /// Add an Account object to the database.
        /// </summary>
        /// <param name="acc"> Account object to add to the database</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool AddAccount(Account acc);

        /// <summary>
        /// Remove an Account object from the database.
        /// </summary>
        /// <param name="acc">The Account object being removed from the database</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool RemoveAccount(Account acc);

        /// <summary>
        /// Update an Account object in the database. Modify the account object, then pass it into this method. The corresponding object in the database will be updated accordingly.
        /// </summary>
        /// <param name="acc">The Account object with modified parameters</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool UpdateAccount(Account acc);
    }
}
