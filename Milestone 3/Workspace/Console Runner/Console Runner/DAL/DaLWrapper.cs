using Class1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.DAL
{
    internal class DaLWrapper
    {
        Context context = new();
        public DaLWrapper()
        {

        }

        public bool userExists(string email)
        {
            if (context.accounts.Find(email) != null)
            {
                return true;
            }
            return false;
        }
        public bool hasPermission(string email, string permission)
        {
            if (context.permissions.Find(email, permission) != null)
            {
                return true;
            }
            return false ;
        }
    }
}
