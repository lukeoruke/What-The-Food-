using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.AccountService.Authentication
{
    public interface IAuthenticationService
    {
        public string GenerateToken(string data);
        public string Decrypt(string encryptedData);
        public bool ValidateToken(string token);
        public string GetUsername(string token);
    }
}
