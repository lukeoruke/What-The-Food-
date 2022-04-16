using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Console_Runner.Logging
{
    public class EFLogGateway : ILogGateway
    {
        public async Task<bool> WriteLogAsync(Log log, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            using ContextLoggingDB efContext = new ContextLoggingDB();
            efContext.Entry(log.UserIdentifier).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            ct.ThrowIfCancellationRequested();
            await efContext.Logs.AddAsync(log, ct);
            await efContext.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> WriteLogsAsync(List<Log> logs, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            using ContextLoggingDB efContext = new ContextLoggingDB();
            foreach (Log log in logs)
            {
                efContext.Entry(log.UserIdentifier).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            }
            ct.ThrowIfCancellationRequested();
            await efContext.Logs.AddRangeAsync(logs, ct);
            await efContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
