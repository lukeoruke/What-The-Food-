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
        public string UserId { get; set; }
        public string UserHash { get; set; }

        public UserIdentifier()
        {

        }
        public UserIdentifier(string userid)
        {
            UserId = userid;
            using(SHA256 sha256 = SHA256.Create())
            {
                UserHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(userid)));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            UserIdentifier objAsPart = obj as UserIdentifier;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public bool Equals(UserIdentifier other)
        {
            return UserId == other.UserId;
        }
    }
}
