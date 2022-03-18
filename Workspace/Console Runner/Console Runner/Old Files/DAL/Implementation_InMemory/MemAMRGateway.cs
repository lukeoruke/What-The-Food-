using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.AMRModel;

namespace Console_Runner.DAL
{
    public class MemAMRGateway : IAMRGateway
    {
        private List<AMR> _amrDB = new();
        public bool AddAMR(AMR amrToAdd)
        {
            _amrDB.Add(amrToAdd);
            return true;
        }
        public bool AMRExists(string email)
        {
            for(int i = 0; i < _amrDB.Count; i++)
            {
                if(_amrDB[i].AccountEmail == email)
                {
                    return true;
                }
            }
            return false;
        }

        public AMR? GetAMR(string email)
        {
            for (int i = 0; i < _amrDB.Count; i++)
            {
                if (_amrDB[i].AccountEmail == email)
                {
                    return _amrDB[i];
                }
            }
            return null;
        }

        public bool RemoveAMR(AMR amrToRemove)
        {
            return _amrDB.Remove(amrToRemove);
        }

        public bool UpdateAMR(AMR amrToUpdate)
        {
            for (int i = 0; i < _amrDB.Count; i++)
            {
                if (_amrDB[i].AccountEmail == amrToUpdate.AccountEmail)
                {
                    _amrDB[i] = amrToUpdate;
                    return true;
                }
            }
            return false;
        }
    }
}
