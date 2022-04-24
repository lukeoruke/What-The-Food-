using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Logging
{
    public class EFUserIdentifierGateway : IUserIDGateway
    {
        public EFUserIdentifierGateway()
        {

        }

        public async Task<UserIdentifier> AddUserIdAsync(string idToAdd, CancellationToken cancellationToken = default)
        {
            using ContextLoggingDB efContext = new ContextLoggingDB();
            UserIdentifier uid = new UserIdentifier(idToAdd);
            cancellationToken.ThrowIfCancellationRequested();
            await efContext.UIDs.AddAsync(uid, cancellationToken);
            await efContext.SaveChangesAsync(cancellationToken);
            return uid;
        }

        public async Task<string?> GetUserHashAsync(string idToGet, CancellationToken cancellationToken = default)
        {
            using ContextLoggingDB efContext = new ContextLoggingDB();
            cancellationToken.ThrowIfCancellationRequested();
            UserIdentifier? uid = await efContext.UIDs.FindAsync(new string[] { idToGet }, cancellationToken);
            return uid?.UserHash;
        }

        public async Task<UserIdentifier?> GetUserIdentifierAsync(string idToGet, CancellationToken cancellationToken = default)
        {
            using ContextLoggingDB efContext = new ContextLoggingDB();
            cancellationToken.ThrowIfCancellationRequested();
            return await efContext.UIDs.FirstOrDefaultAsync((c => c.UserId == idToGet), cancellationToken);
        }

        public async Task<bool> RemoveUserIdAsync(string idToRemove, CancellationToken cancellationToken = default)
        {
            using ContextLoggingDB efContext = new ContextLoggingDB();
            UserIdentifier uid = new UserIdentifier(idToRemove);
            cancellationToken.ThrowIfCancellationRequested();
            efContext.UIDs.Remove(uid);
            await efContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
