using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Logging;

namespace Console_Runner.DAL
{
    public class EFLogGateway : ILogGateway
    {
        private readonly Context _efContext;

        public EFLogGateway()
        {
            _efContext = new Context();
        }

        public bool WriteLog(Logs toLog)
        {
            _efContext.Logs.Add(toLog);
            return true;
        }
    }
}
