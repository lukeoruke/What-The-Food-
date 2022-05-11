using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Console_Runner.Logging;

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


            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            logger.DefaultTimeOut = 5000;
            IFormCollection formData = Request.Form;
            
            try
            {
                account = await _accountDBOperations.SignInAsync(formData["email"].ToString(), formData["password"].ToString());
                if (account != null)
                {
                    string jwtToken = _JWTAuthenticationService.GenerateToken(account.Email);
                    string json = "{" + "\"token\": " + $"\"{jwtToken}\"" + "}";
                    await _accountDBOperations.StartSessionAsync(account.UserID, jwtToken);
                    if (account.CollectData)
                    {
                        logger.UserEmail = account.Email;
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                    $"User {account.UserID} Logged in successfully");
                    }
                    else
                    {
                        _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                            "Anonymous user logged in successfully");
                    }
                    return json;
                }
                return "{" + "\"token\": \"\"}";
            }
            catch (FileNotFoundException e)
            {
                _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                        $"A user could not login successfully");
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
