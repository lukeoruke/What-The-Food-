using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;

namespace Microservice.AccountLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("MyAllowSpecificOrigins")] fts
    public class AccountLoginController : ControllerBase
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");
        private readonly IAMRGateway _aMRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private Account? account;




        [HttpPost]
        public async Task<string> Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations
                (_accountAccess, _permissionService, _flagGateway, _aMRGateway, _EFActiveSessionTrackerGateway);
            Console.WriteLine("SUCCESSS!!!");
            Console.WriteLine("Received Post from LoginController");
            //Console.WriteLine(Request.Form("username"));

            IFormCollection formData = Request.Form;

            Console.WriteLine(formData["email"]);
            Console.WriteLine(formData["password"]);

            try
            {
               

                account = await _accountDBOperations.SignInAsync(formData["email"].ToString(), formData["password"].ToString());
                if (account != null)
                {
                    string jwtToken = _JWTAuthenticationService.GenerateToken(account.Email);
                    Console.WriteLine("validToken VVV");
                    Console.WriteLine(_JWTAuthenticationService.ValidateToken(jwtToken));
                    string json = "{" + "\"token\": " + $"\"{jwtToken}\"" + "}";
                    await _accountDBOperations.StartSessionAsync(account.UserID, jwtToken);
                    return json;
                    Console.WriteLine("ACCOUNT HAD SOME DATA " + account.ToString());
                }
                return null;
                
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
