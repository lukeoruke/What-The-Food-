using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Console_Runner.Logging
{
    public class EFLogGateway : ILogGateway
    {
        private readonly ContextLoggingDB _efContext;

        public EFLogGateway()
        {
            _efContext = new ContextLoggingDB();
        }

        public bool WriteLog(Log toLog)
        {
            _efContext.Logs.Add(toLog);
            return true;
        }
    }
}
