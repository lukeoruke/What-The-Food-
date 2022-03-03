using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.DAL
{
    public class EFLogGateway : ILogGateway
    {
        private readonly Context _efContext;

        public EFLogGateway(Context dbContext)
        {
            _efContext = dbContext;
        }
        public bool WriteLog(string toLog)
        {
            _efContext.Logs.Add()
        }
    }
}
