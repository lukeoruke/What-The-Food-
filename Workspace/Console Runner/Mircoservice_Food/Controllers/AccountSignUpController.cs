using Console_Runner.AccountService;
using Microsoft.AspNetCore.Mvc;
using  Microservice.AccountLogin;
using Console_Runner.Logging;


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

            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            logger.DefaultTimeOut = 5000;
            IFormCollection formData = Request.Form;

            try
            {
                AMR amr = new AMR();
                Account account = new Account();
                account.Email = formData["email"].ToString();
                logger.UserEmail = account.Email;
                account.Password = formData["password"].ToString();
                string name = formData["name"];
                string[] fullName = name.Split(" ");
                account.FName = fullName[0];
                account.LName = fullName[1];
                account.NewsBias = 1;
                await _accountDBOperations.UserSignUpAsync(account, logger);

                _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                               $"User signed up successfully.");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
