using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.AccountService;
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
            AccountDBOperations _accountDBOperations = new AccountDBOperations
                (_accountAccess, _permissionService, _flagGateway);
            Console.WriteLine("SUCCESSS!!!");
            Console.WriteLine("Received Post from LoginController");
            //Console.WriteLine(Request.Form("username"));

            IFormCollection formData = Request.Form;

            Console.WriteLine(formData["email"]);
            Console.WriteLine(formData["password"]);



            try
            {
               

                Account account = _accountDBOperations.SignIn(formData["email"].ToString(), formData["password"].ToString());
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
