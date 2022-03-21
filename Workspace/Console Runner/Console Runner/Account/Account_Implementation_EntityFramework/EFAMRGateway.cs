using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.AccountService
{
    public class EFAMRGateway : IAMRGateway
    {
        private readonly ContextAccountDB _efContext;

        public EFAMRGateway()
        {
            _efContext = new ContextAccountDB();
        }
        public async Task<bool> AddAMRAsync(AMR amrToAdd)
        {
            try
            {
                await _efContext.AMRs.AddAsync(amrToAdd);
                await _efContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AMRExistsAsync(int userID)
        {
            return await _efContext.AMRs.FindAsync(userID) != null;
        }

        public async Task<AMR?> GetAMRAsync(int userID)
        {
            try
            {
                AMR? foundAMR = await _efContext.AMRs.FindAsync(userID);
                if (foundAMR != null) return foundAMR;
                else throw new Exception("AMR could not be found");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> RemoveAMRAsync(AMR amrToRemove)
        {
            try
            {
                if (await AMRExistsAsync(amrToRemove.UserID))
                {
                    _efContext.Remove(amrToRemove);
                    await _efContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAMRAsync(AMR amrToUpdate)
        {
            try
            {
                _efContext.AMRs.Update(amrToUpdate);
                await _efContext.SaveChangesAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
