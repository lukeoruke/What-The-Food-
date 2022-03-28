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

        public string AddUserId(string idToAdd)
        {
            UserIdentifier uid = new UserIdentifier(idToAdd);
            _efContext.Add(uid);
            return uid.UserHash;
        }

        public string? GetUserHash(string idToGet)
        {
            UserIdentifier? uid = _efContext.UIDs.Find(idToGet);
            if (uid == null) return null;
            else return uid.UserHash;
        }

        public bool RemoveUserId(string idToRemove)
        {
            UserIdentifier uid = new UserIdentifier(idToRemove);
            _efContext.UIDs.Remove(uid);
            return true;
        }
    }
}
