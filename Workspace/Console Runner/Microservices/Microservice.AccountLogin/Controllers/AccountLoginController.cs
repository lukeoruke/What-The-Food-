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
    //[EnableCors("MyAllowSpecificOrigins")] fts
    public class AccountLoginController : ControllerBase
    {
        static IAccountGateway accountGateway = new EFAccountGateway();
        static IPermissionGateway efPermissionGateway = new EFPermissionGateway();
        static PermissionService permService = new PermissionService(efPermissionGateway);
        static IlogGateway logAccess = new EFLogGateway();
        static Logging logger = new Logging(logAccess);
        static UM um = new(accountGateway, permService, logger);

        [HttpGet]
        //place methods here
        public async Task<ActionResult<AccountLogin>> Get()
        {
            var user = new AccountLogin();
            Console.WriteLine("asdkfhjaweklfhjasdfhlafhlakfha2");
            user.email = "something@testEmail.com";
            return Ok(user);
        }

        [HttpPost]
        public void Post()
        {
            Console.WriteLine("SUCCESSS!!!");
            Console.WriteLine("Received Post from LoginController");
            //Console.WriteLine(Request.Form("username"));

            IFormCollection formData = Request.Form;

            Console.WriteLine(formData["email"]);
            Console.WriteLine(formData["password"]);



            try
            {
               

                Account account = um.SignIn(formData["email"].ToString(), formData["password"].ToString());
                if (account != null)
                {

                }
                
                Console.WriteLine(account.ToString());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
