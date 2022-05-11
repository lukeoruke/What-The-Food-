using Console_Runner.Logging;

namespace Console_Runner.AccountService
{
    public class EFAMRGateway : IAMRGateway
    {
        private readonly ContextAccountDB _efContext;

        public EFAMRGateway()
        {
            _efContext = new ContextAccountDB();
        }
        public async Task<bool> AddAMRAsync(AMR amrToAdd, LogService? logService = null)
        {
            try
            {
                await _efContext.AMRs.AddAsync(amrToAdd);
                await _efContext.SaveChangesAsync();
                if (logService?.UserEmail != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Created AMR for user {amrToAdd.UserID}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AMRExistsAsync(int userID, LogService? logService = null)
        {
            var toReturn = (await _efContext.AMRs.FindAsync(userID)) != null;
            if (logService?.UserEmail != null)
            {
                _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                   $"Retrieved AMR for user {userID}");
            }
            return toReturn;
        }

        public async Task<AMR?> GetAMRAsync(int userID, LogService? logService = null)
        {
            try
            {
                AMR? foundAMR = await _efContext.AMRs.FindAsync(userID);
                if (foundAMR != null)
                {
                    if (logService?.UserEmail != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Retrieved AMR for user {userID}");
                    }
                    return foundAMR;
                }
                else
                {
                    if (logService?.UserEmail != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Could not retrieve AMR for user {userID}");
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> RemoveAMRAsync(AMR amrToRemove, LogService? logService = null)
        {
            try
            {
                if(amrToRemove == null)
                {
                    return true;
                }
                if (await AMRExistsAsync(amrToRemove.UserID))
                {
                    _efContext.Remove(amrToRemove);
                    await _efContext.SaveChangesAsync();
                    if (logService?.UserEmail != null)
                    {
                        _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                           $"Removed AMR for user {amrToRemove.UserID}");
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAMRAsync(AMR amrToUpdate, LogService? logService = null)
        {
            try
            {
                _efContext.AMRs.Update(amrToUpdate);
                await _efContext.SaveChangesAsync(true);
                if (logService?.UserEmail != null)
                {
                    _ = logService.LogWithSetUserAsync(Logging.LogLevel.Info, Category.DataStore, DateTime.Now,
                                                       $"Updated AMR for user {amrToUpdate.UserID}");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
