using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.AMRModel;

namespace Console_Runner.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAMRRepo
    {
        public bool AMRExists(string email);
        public AMR? GetAMR(string email);
        public bool AddAMR(AMR amrToAdd);
        public bool RemoveAMR(AMR amrToRemove);
        public bool UpdateAMR(AMR amrToUpdate);
    }
}
