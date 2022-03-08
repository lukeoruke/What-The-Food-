using Console_Runner.DAL;
using Console_Runner.Logging;
using Console_Runner.User_Management;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.AccountLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class AccountLoginController : ControllerBase
    {
        /// <summary>
        /// Establish Connection To Interfaces that all have access to database
        /// </summary>

        static IAccountGateway accountGateway = new EFAccountGateway();
        static IPermissionGateway efPermissionGateway = new EFPermissionGateway();
        static PermissionService permService = new PermissionService(efPermissionGateway);
        static IlogGateway logAccess = new EFLogGateway();
        static Logging logger = new Logging(logAccess);
        static UM um = new(accountGateway, permService, logger);

        /// <summary>
        /// HTTP Get Request for Account Login Microservice
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<AccountLogin>> Get()
        {
            var user = new AccountLogin();
            user.email = "something@testEmail.com";
            return Ok(user);
        }
        /// <summary>
        /// HTTP Post Request for Account Login Microservice
        /// </summary>
        [HttpPost]
        public void Post()
        {
            Console.WriteLine("Received Post from LoginController");
            //Console.WriteLine(Request.Form("username"));

            IFormCollection formData = Request.Form;

            //Console.WriteLine(formData["email"]);
            //Console.WriteLine(formData["password"]);

            try
            {
                Account account = new Account();
                account.Email = formData["email"].ToString();
                account.Password = formData["password"].ToString();

                um.UserSignUp(account);
                
                Console.WriteLine(account.ToString());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
