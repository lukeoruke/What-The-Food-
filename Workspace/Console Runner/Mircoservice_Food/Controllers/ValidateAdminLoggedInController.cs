using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Console_Runner.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateAdminLoggedInController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");

        [HttpGet]
        public async Task<ActionResult<bool>> Get(string token, int time, string previousViewName, string currentViewName)
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            logger.DefaultTimeOut = 5000;
            bool tokenIsValid = await _accountDBOperations.ValidateToken(token);
            int activeUserId = await _accountDBOperations.GetActiveUserAsync(token);
            bool tokenBelongsToAdmin = _accountDBOperations.IsAdmin(activeUserId);
            if (tokenIsValid)
            {
                Account? activeUser = await _accountDBOperations.GetUserAccountAsync(activeUserId);
                // token is valid and user is admin
                if (tokenBelongsToAdmin)
                {
                    if (activeUser?.CollectData ?? false)
                    {
                        logger.UserEmail = activeUser.Email;
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.View, DateTime.Now,
                                                       $"Admin {activeUserId} navigated from {previousViewName} after {time} seconds to {currentViewName}.");
                    }
                    else
                    {
                        _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Info, Category.View, DateTime.Now,
                                            $"Unknown admin navigated from {previousViewName} after {time} seconds to {currentViewName}.");
                    }
                }
                // token is valid and user is not an admin
                else
                {
                    if (activeUser?.CollectData ?? false)
                    {
                        logger.UserEmail = activeUser.Email;
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Warning, Category.View, DateTime.Now,
                                                       $"Unauthorized user {activeUserId} attempted to navigate from {previousViewName} to {currentViewName}.");
                    }
                    else
                    {
                        _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Warning, Category.View, DateTime.Now,
                                            $"Unknown unauthorized user attempted to navigate from {previousViewName} to {currentViewName}.");
                    }
                }
            }
            else
            {
                _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Warning, Category.View, DateTime.Now,
                                    $"User attempted to navigate from {previousViewName} to {currentViewName} with an invalid token.");
            }
            return tokenIsValid && tokenBelongsToAdmin;
        }
    }
}





