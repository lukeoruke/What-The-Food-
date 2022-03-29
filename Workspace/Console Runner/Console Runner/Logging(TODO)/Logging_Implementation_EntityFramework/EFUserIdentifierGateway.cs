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

        public async Task<string> AddUserIdAsync(string idToAdd)
        {
            UserIdentifier uid = new UserIdentifier(idToAdd);
            await _efContext.UIDs.AddAsync(uid);
            await _efContext.SaveChangesAsync();
            return uid.UserHash;
        }

        public async Task<string?> GetUserHashAsync(string idToGet)
        {
            UserIdentifier? uid = await _efContext.UIDs.FindAsync(idToGet);
            return uid?.UserHash;
        }

        public async Task<bool> RemoveUserIdAsync(string idToRemove)
        {
            UserIdentifier uid = new UserIdentifier(idToRemove);
            _efContext.UIDs.Remove(uid);
            await _efContext.SaveChangesAsync();
            return true;
        }
    }
}
