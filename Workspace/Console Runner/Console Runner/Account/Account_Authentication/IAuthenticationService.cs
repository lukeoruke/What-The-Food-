using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Account.Account_Authentication
{
    internal interface IAuthenticationService
    {
        public string generateToken(string data, ClaimsIdentity claimsIdentity);
        public string Decrypt(string encryptedData);
        public bool ValidateToken(string token);
        public string getUsername(string token);
    }
}
