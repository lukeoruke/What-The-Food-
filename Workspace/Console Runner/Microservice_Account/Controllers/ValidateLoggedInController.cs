using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Console_Runner.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateLoggedInController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");

        [HttpGet]
        public async Task<ActionResult<string>> Get(string token, int time, string previousViewName, string currentViewName)
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            logger.DefaultTimeOut = 5000;
            bool isValid = await _accountDBOperations.ValidateToken(token);
            if (isValid)
            {
                int activeUserId = await _accountDBOperations.GetActiveUserAsync(token);
                Account? activeUser = await _accountDBOperations.GetUserAccountAsync(activeUserId);
                if(activeUser?.CollectData ?? false)
                {
                    logger.UserEmail = activeUser.Email;
                    _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.View, DateTime.Now,
                                                   $"User {activeUserId} navigated from {previousViewName} after {time} seconds to {currentViewName}.");
                }
                else
                {
                    _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Info, Category.View, DateTime.Now,
                                        $"Unknown user navigated from {previousViewName} after {time} seconds to {currentViewName}.");
                }
            }
            else
            {
                _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Warning, Category.View, DateTime.Now,
                                    $"User attempted to navigate from {previousViewName} to {currentViewName} with an invalid token.");
            }
            Console.WriteLine(isValid);
            return isValid.ToString();
        }
    }
}





