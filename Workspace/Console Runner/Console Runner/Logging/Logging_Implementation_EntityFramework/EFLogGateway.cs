using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Console_Runner.Logging
{
    public class EFLogGateway : ILogGateway
    {

        public EFLogGateway()
        {
        }

        public async Task<bool> WriteLogAsync(Log toLog, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            using ContextLoggingDB efContext = new ContextLoggingDB();
            efContext.Entry(toLog.UserIdentifier).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            await efContext.Logs.AddAsync(toLog, ct);
            await efContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
