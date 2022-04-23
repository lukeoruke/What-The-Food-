using Console_Runner.Logging;

namespace Console_Runner.AccountService
{

    public interface IAMRGateway
    {
        public Task<bool> AMRExistsAsync(int userID, LogService? logService = null);
        public Task<AMR?> GetAMRAsync(int userID, LogService? logService = null);
        public Task<bool> AddAMRAsync(AMR amrToAdd, LogService? logService = null);
        public Task<bool> RemoveAMRAsync(AMR amrToRemove, LogService? logService = null);
        public Task<bool> UpdateAMRAsync(AMR amrToUpdate, LogService? logService = null);
    }
}
