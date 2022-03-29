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

        public async Task<bool> WriteLogAsync(Log toLog, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await _efContext.Logs.AddAsync(toLog, ct);
            await _efContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
