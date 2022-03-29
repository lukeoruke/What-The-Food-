using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Logging.Testing
{
    public class MemUserIdentifierGateway : IUserIDGateway
    {
        private List<UserIdentifier> _uidsDB;

        public MemUserIdentifierGateway()
        {
            _uidsDB = new List<UserIdentifier>();
        }
        public async Task<string> AddUserIdAsync(string idToAdd)
        {
            UserIdentifier newUID = new UserIdentifier(idToAdd);
            if (!_uidsDB.Contains(newUID))
            {
                _uidsDB.Add(newUID);
            }
            return newUID.UserHash;
        }

        public async Task<string?> GetUserHashAsync(string idToGet)
        {
            return _uidsDB.Find(uid => uid.UserId == idToGet)?.UserHash;
        }

        public async Task<bool> RemoveUserIdAsync(string idToRemove)
        {
            _uidsDB.RemoveAt(_uidsDB.FindIndex(uid => uid.UserId == idToRemove));
            return true;
        }
    }
}
