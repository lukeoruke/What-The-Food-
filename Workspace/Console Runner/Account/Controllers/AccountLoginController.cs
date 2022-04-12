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
        private readonly IConfiguration _configuration;
        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        
        public AccountLoginController(IConfiguration config)
        {
            _configuration = config;
        }

        //place methods here
        [HttpGet]
        public IActionResult TestOne()
        {
            var user = new AccountLogin();
            Console.WriteLine("asdkfhjaweklfhjasdfhlafhlakfha2");
            user.email = "something@testEmail.com";
            return Ok(user);
        }

        [HttpPost]
        public IActionResult TestTwo()
        {
            Console.WriteLine("Post received");
            AccountDBOperations accountService = new AccountDBOperations
                (_accountAccess, _permissionService, _flagGateway);
            IAuthenticationService authenticationService = new JWTAuthenticationService(_configuration["JWTSecret"].ToString());

            IFormCollection formData = Request.Form;

            try
            {
                Console.WriteLine($"email: {formData["email"].ToString()} pass: {formData["password"].ToString()}");
                Console_Runner.AccountService.Account? account = accountService.SignInAsync(formData["email"].ToString(), formData["password"].ToString()).Result;
                if (account != null)
                {
                    return Ok(authenticationService.GenerateToken(account.Email));   
                }
                else
                {
                    return new UnauthorizedResult();
                }
                Console.WriteLine(account?.ToString() ?? "Account not found.");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                return new StatusCodeResult(500);
            }
        }
    }
}
