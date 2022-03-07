using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.AMRModel;

namespace Console_Runner.DAL
{
    public interface IAMRGateway
    {
        /// <summary>
        /// Verifies whether an AMR associated with the given user email exists in the database.
        /// </summary>
        /// <param name="email">The email address to find an associated AMR with.</param>
        /// <returns>True if an AMR associated with the given email address exists.</returns>
        public bool AMRExists(string email);
        /// <summary>
        /// Gets the AMR associated with the given user email from the database.
        /// </summary>
        /// <param name="email">The email address that the AMR to get is associated with.</param>
        /// <returns>The AMR object associated with the given user email if it exists, otherwise null.</returns>
        public AMR? GetAMR(string email);
        /// <summary>
        /// Adds an AMR object to the database.
        /// </summary>
        /// <param name="amrToAdd">The AMR to add to the database.</param>
        /// <returns>True if the AMR was added to the database, otherwise false.</returns>
        public bool AddAMR(AMR amrToAdd);
        /// <summary>
        /// Removes the given AMR from the database.
        /// </summary>
        /// <param name="amrToRemove">The AMR to remove from the database.</param>
        /// <returns>True if the AMR was removed from the database, otherwise false.</returns>
        public bool RemoveAMR(AMR amrToRemove);
        /// <summary>
        /// Updates the AMR in the database with the corresponding email, with new information in the given AMR object.
        /// </summary>
        /// <param name="amrToUpdate">The AMR object with the new data to add.</param>
        /// <returns>True if the corresponding AMR in the database was updated, false otherwise.</returns>
        public bool UpdateAMR(AMR amrToUpdate);
    }
}
