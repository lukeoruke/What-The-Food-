using Console_Runner.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.DAL
{
    public class MemLogGateway : ILogGateway
    {
        private List<Logs> _logsDB;

        public MemLogGateway()
        {
            _logsDB = new List<Logs>();
        }
        public bool WriteLog(Logs toLog)
        {
            _logsDB.Add(toLog);
            return true;
        }
    }
}
