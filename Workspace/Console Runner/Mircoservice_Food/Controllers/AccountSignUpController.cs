using Console_Runner.AccountService;
using Microsoft.AspNetCore.Mvc;
using  Microservice.AccountLogin;
namespace Microservice.AccountLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("MyAllowSpecificOrigins")] fts
    public class AccountSignUpController : ControllerBase
    {

        private const string UM_CATEGORY = "Data Store";
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _aMRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();

        [HttpPost]
        public async void Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations
                (_accountAccess, _permissionService, _flagGateway, _aMRGateway, _EFActiveSessionTrackerGateway);
 
            Console.WriteLine("SUCCESSS!!!");
            Console.WriteLine("Received Post from LoginController");
            //Console.WriteLine(Request.Form("username"));
            Console.WriteLine("Above read form data");

            IFormCollection formData = Request.Form;
            Console.WriteLine("UNDER read form data");

            Console.WriteLine(formData["email"]);
            Console.WriteLine(formData["password"]);

            try
            {
                AMR amr = new AMR();
                Account account = new Account();
                account.Email = formData["email"].ToString();
                account.Password = formData["password"].ToString();
                string name = formData["name"];
                string[] fullName = name.Split(" ");
                account.FName = fullName[0];
                account.LName = fullName[1];
                await _accountDBOperations.UserSignUpAsync(account);

                Console.WriteLine(account.ToString());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
