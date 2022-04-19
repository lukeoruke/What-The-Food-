using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public interface IFlagGateway
    {
        /// <summary>
        /// Gets N(take) flags that belong to the userID provided while skipping over first m(skip) results. 
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved</param>
        /// <param name="skip">The number of entries to skip before pulling</param>
        /// <param name="take">The number of entries to return</param>
        /// <returns>A list containing the food flags associated with the user</returns>
        public Task<List<FoodFlag>> GetNAccountFlagsAsync(int userID, int skip, int take, LogService? logService = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved</param>
        /// <param name="ingredientID">The ingredients ID</param>
        /// <returns>true if the user has a flag associated with the provided ingredient ID</returns>
        public Task<bool> AccountHasFlagAsync(int userID, int ingredientID, LogService? logService = null);
        /// <summary>
        /// Removes a flag from the users account
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved </param>
        /// <param name="ingredientID">The ingredients ID</param>
        /// <returns></returns>
        public Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID, LogService? logService = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID">The user whos ID's are being retrieved</param>
        /// <returns>A list of all flags associated with the users account</returns>
        public Task<List<FoodFlag>> GetAllAccountFlagsAsync(int userID, LogService? logService = null);
        /// <summary>
        /// adds a flag to the users account
        /// </summary>
        /// <param name="flag">The flag to be added</param>
        /// <returns>true if successful false otherwise</returns>
        public Task<bool> AddFlagAsync(FoodFlag flag, LogService? logService = null);
    }
}
