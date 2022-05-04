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
        private Account? account = new Account();


        /*        [HttpGet]
                //place methods here
                //THIS IS DEAD CODE?????? I THINK????? ASK TYLER
                public async Task<ActionResult<AccountLogin>> Get()
                {
                    var user = new AccountLogin();
                    Console.WriteLine("asdkfhjaweklfhjasdfhlafhlakfha2");
                    user.email = "something@testEmail.com";
                    return Ok(user);
                }*/

        [HttpPost]
        public async void Post()
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
                    
                    Console.WriteLine("ACCOUNT HAD SOME DATA " + account.ToString());
                }
                
                
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [HttpGet]
        public string GET()
        {
            string jwtToken = _JWTAuthenticationService.GenerateToken(account.Email);
            Console.WriteLine("validToken VVV");
            Console.WriteLine(_JWTAuthenticationService.ValidateToken(jwtToken));
            string json = "{"+"\"token\": " + $"\"{jwtToken}\"" +"}";
            _EFActiveSessionTrackerGateway.StartSessionAsync(account.UserID, jwtToken);
            return json;
        }
    }
}
