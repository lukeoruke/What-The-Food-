using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public class MemAMRGateway : IAMRGateway
    {
        private List<AMR> _amrDB = new();

        public async Task<bool> AddAMRAsync(AMR amrToAdd, LogService? logService = null)
        {
            try
            {
                _amrDB.Add(amrToAdd);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Created AMR for user {amrToAdd.UserID}");
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AMRExistsAsync(int userID, LogService? logService = null)
        {
            foreach (AMR amr in _amrDB)
            {
                if(amr.UserID == userID)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved AMR for user {userID}");
                    }
                    return true;
                }
            }
            return false;
        }

        public async Task<AMR?> GetAMRAsync(int userID, LogService? logService = null)
        {
            foreach (AMR amr in _amrDB)
            {
                if (amr.UserID == userID)
                {
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved AMR for user {userID}");
                    }
                    return amr;
                }
            }
            return null;
        }

        public async Task<bool> RemoveAMRAsync(AMR amrToRemove, LogService? logService = null)
        {
            try
            {
                _amrDB.Remove(amrToRemove);
                if (logService?.UserID != null)
                {
                    _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Removed AMR for user {amrToRemove.UserID}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAMRAsync(AMR amrToUpdate, LogService? logService = null)
        {
            for (int index = 0; index < _amrDB.Count; index++)
            {
                if(_amrDB[index].UserID == amrToUpdate.UserID)
                {
                    _amrDB[index] = amrToUpdate;
                    if (logService?.UserID != null)
                    {
                        _ = logService.LogWithSetUserAsync(LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Updated AMR for user {amrToUpdate.UserID}");
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
