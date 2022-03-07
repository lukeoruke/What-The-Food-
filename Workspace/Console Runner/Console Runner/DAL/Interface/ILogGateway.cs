using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Logging;

namespace Console_Runner.DAL
{
    public interface IlogGateway
    {
        public bool WriteLog(Logs toLog);

    }
}
