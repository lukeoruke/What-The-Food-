

namespace Console_Runner.AccountService
{

    public interface IAMRGateway
    {
        public Task<bool> AMRExistsAsync(int userID);
        public Task<AMR?> GetAMRAsync(int userID);
        public Task<bool> AddAMRAsync(AMR amrToAdd);
        public Task<bool> RemoveAMRAsync(AMR amrToRemove);
        public Task<bool> UpdateAMRAsync(AMR amrToUpdate);
    }
}
