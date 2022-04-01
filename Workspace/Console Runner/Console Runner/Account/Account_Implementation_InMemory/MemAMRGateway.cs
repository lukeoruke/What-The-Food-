
namespace Console_Runner.AccountService
{
    public class MemAMRGateway : IAMRGateway
    {
        private List<AMR> _amrDB = new();

        public async Task<bool> AddAMRAsync(AMR amrToAdd)
        {
            try
            {
                _amrDB.Add(amrToAdd);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AMRExistsAsync(int userID)
        {
            foreach (AMR amr in _amrDB)
            {
                if(amr.UserID == userID)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<AMR?> GetAMRAsync(int userID)
        {
            foreach (AMR amr in _amrDB)
            {
                if (amr.UserID == userID)
                {
                    return amr;
                }
            }
            return null;
        }

        public async Task<bool> RemoveAMRAsync(AMR amrToRemove)
        {
            try
            {
                _amrDB.Remove(amrToRemove);
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAMRAsync(AMR amrToUpdate)
        {
            foreach (AMR amr in _amrDB)
            {
                if (amr.UserID == amrToUpdate.UserID)
                {
                    await RemoveAMRAsync(amr);
                    await AddAMRAsync(amrToUpdate);
                    return true;
                }
            }
            return false;
        }
    }
}
