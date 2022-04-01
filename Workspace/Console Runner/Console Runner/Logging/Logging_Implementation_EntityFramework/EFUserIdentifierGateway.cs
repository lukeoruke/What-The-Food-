using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Logging
{
    public class EFUserIdentifierGateway : IUserIDGateway
    {
        private readonly ContextLoggingDB _efContext;

        public EFUserIdentifierGateway()
        {
            _efContext = new ContextLoggingDB();
        }

        public async Task<string> AddUserIdAsync(string idToAdd, CancellationToken cancellationToken = default)
        {
            UserIdentifier uid = new UserIdentifier(idToAdd);
            cancellationToken.ThrowIfCancellationRequested();
            await _efContext.UIDs.AddAsync(uid, cancellationToken);
            await _efContext.SaveChangesAsync(cancellationToken);
            return uid.UserHash;
        }

        public async Task<string?> GetUserHashAsync(string idToGet, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            UserIdentifier? uid = await _efContext.UIDs.FindAsync(new string[] { idToGet }, cancellationToken);
            return uid?.UserHash;
        }

        public async Task<bool> RemoveUserIdAsync(string idToRemove, CancellationToken cancellationToken = default)
        {
            UserIdentifier uid = new UserIdentifier(idToRemove);
            cancellationToken.ThrowIfCancellationRequested();
            _efContext.UIDs.Remove(uid);
            await _efContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
