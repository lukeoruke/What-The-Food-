using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Console_Runner.Account.Account_Authentication
{
    public class JWTAuthenticationService
    {
        private readonly string _key;


        public JWTAuthenticationService(string configuration)
        {
            _key = configuration;
        }



        public string generateToken(string data, ClaimsIdentity claimsIdentity)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_key);
            var segments = new List<string>();

            claimsIdentity.AddClaim(new Claim("username", data));
            claimsIdentity.AddClaim(new Claim("iat", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()));

            var header = new { alg = "HS256", typ = "JWT" };
            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(header));
            JwtPayload payload = new JwtPayload(claimsIdentity.Claims);

            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload.SerializeToJson());

            segments.Add(Convert.ToBase64String(headerBytes));
            segments.Add(Convert.ToBase64String(payloadBytes));
            //segments.Add(Encoding.UTF8.GetString(Base64UrlDecode(BYTE ARRAY OF THE DATA STRING HERE)));

            var stringToSign = string.Join(".", segments.ToArray());

            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            var sha = new HMACSHA256(keyBytes);
            byte[] signature = sha.ComputeHash(bytesToSign);
            segments.Add(Convert.ToBase64String(signature));

            return string.Join(".", segments.ToArray());
        }

        public bool ValidateToken(string token)
        {
            try
            {


                var parts = token.Split('.');
                var header = parts[0];
                var payload = parts[1];
                byte[] crypto = Convert.FromBase64String(parts[2]);



                var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
                var keyBytes = Encoding.UTF8.GetBytes(_key);

                var sha = new HMACSHA256(keyBytes);
                byte[] signature = sha.ComputeHash(bytesToSign);
                var decodedCrypto = Convert.ToBase64String(crypto);
                var decodedSignature = Convert.ToBase64String(signature);



                if (decodedCrypto != decodedSignature)
                {
                    return false;
                }


                return true;
            }
            catch
            {
                return false;
            }

        }


        //TODO: FIX
        public string Decrypt(string token)
        {

            return "";
        }


        public string getUsername(string token)
        {

            var parts = token.Split('.');
            var header = parts[0];
            var payload = parts[1];

            var data = Convert.FromBase64String((string)payload);

            var dataString = Encoding.UTF8.GetString(data);

            var obj = JwtPayload.Deserialize(dataString);
            foreach (Claim claim in obj.Claims)
            {
                if (claim.Type == "username")
                {
                    return claim.Value;
                }
            }
            return "";

        }

    }
}
