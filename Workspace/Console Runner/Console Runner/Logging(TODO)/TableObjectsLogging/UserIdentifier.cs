using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Console_Runner.Logging
{
    public class UserIdentifier
    {
        public string UserId { get; }
        public string UserHash { get; }

        public UserIdentifier(string userid)
        {
            UserId = userid;
            using(SHA256 sha256 = SHA256.Create())
            {
                UserHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(userid)));
            }
        }
    }
}
