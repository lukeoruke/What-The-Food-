using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.AMRModel;

namespace Console_Runner.DAL
{
    public class EFAMRGateway : IAMRGateway
    {
        private readonly Context _efContext;

        public EFAMRGateway(Context dbContext)
        {
            _efContext = dbContext;
        }
        public bool AddAMR(AMR amrToAdd)
        {
            try
            {
                _efContext.AMRs.Add(amrToAdd);
                _efContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AMRExists(string email)
        {
            return _efContext.AMRs.Find(email) != null;
        }

        public AMR? GetAMR(string email)
        {
            try
            {
                AMR? foundAMR = _efContext.AMRs.Find(email);
                if (foundAMR != null) return foundAMR;
                else throw new Exception("AMR could not be found");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool RemoveAMR(AMR amrToRemove)
        {
            try
            {
                if (AMRExists(amrToRemove.AccountEmail))
                {
                    _efContext.Remove(amrToRemove);
                    _efContext.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateAMR(AMR amrToUpdate)
        {
            try
            {
                _efContext.AMRs.Update(amrToUpdate);
                _efContext.SaveChanges(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
