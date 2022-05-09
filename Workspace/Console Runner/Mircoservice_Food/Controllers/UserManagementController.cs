using Microsoft.AspNetCore.Mvc;
using Console_Runner.Account;
using Console_Runner.AccountService;
using Console_Runner.Logging;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Account acc, string token)
        {
            AccountDBOperations accountService = new AccountDBOperations(new EFAccountGateway(),
                                                                 new EFAuthorizationGateway(),
                                                                 new EFFlagGateway(),
                                                                 new EFAMRGateway(),
                                                                 new EFActiveSessionTrackerGateway());
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: get userID from token and set logger id
            logger.DefaultTimeOut = 5000;
            string strippedToken = token.Replace("\"", "");
            int activeUserId;
            if(await accountService.ValidateToken(strippedToken))
            {
                activeUserId = await accountService.GetActiveUserAsync(strippedToken);
                logger.UserID = activeUserId.ToString();
            }
            else
            {
                _ = await logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                          $"UserManagementController.AddUser: Received invalid JWT token.");
                return new UnprocessableEntityResult();
            }
            if (accountService.IsAdmin(activeUserId))
            {
                try
                {
                    var successfullyAdded = await accountService.UserSignUpAsync(acc, logger);
                    if (successfullyAdded)
                    {
                        return new OkResult();
                    }
                    else
                    {
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] Account acc, string token)
        {
            AccountDBOperations accountService = new AccountDBOperations(new EFAccountGateway(),
                                                                 new EFAuthorizationGateway(),
                                                                 new EFFlagGateway(),
                                                                 new EFAMRGateway(),
                                                                 new EFActiveSessionTrackerGateway());
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: get userID from token and set logger id
            logger.DefaultTimeOut = 5000;
            string strippedToken = token.Replace("\"", "");
            int activeUserId;
            if (await accountService.ValidateToken(strippedToken))
            {
                activeUserId = await accountService.GetActiveUserAsync(strippedToken);
                logger.UserID = activeUserId.ToString();
            }
            else
            {
                _ = await logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                          $"UserManagementController.AddUser: Received invalid JWT token.");
                return new UnprocessableEntityResult();
            }
            if (accountService.IsAdmin(activeUserId))
            {
                Account? adminAccount = await accountService.GetUserAccountAsync(activeUserId);
                bool success = await accountService.UserUpdateDataAsync(adminAccount!, acc.UserID, acc.FName, acc.LName, acc.Password, logger);
                if (success)
                {
                    return new OkResult();
                }
                else
                {
                    return new BadRequestResult();
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> RemoveUser(int userId, string token)
        {
            AccountDBOperations accountService = new AccountDBOperations(new EFAccountGateway(),
                                                                 new EFAuthorizationGateway(),
                                                                 new EFFlagGateway(),
                                                                 new EFAMRGateway(),
                                                                 new EFActiveSessionTrackerGateway());
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: get userID from token and set logger id
            logger.DefaultTimeOut = 5000;
            string strippedToken = token.Replace("\"", "");
            int activeUserId;
            if (await accountService.ValidateToken(strippedToken))
            {
                activeUserId = await accountService.GetActiveUserAsync(strippedToken);
                logger.UserID = activeUserId.ToString();
            }
            else
            {
                _ = await logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                          $"UserManagementController.AddUser: Received invalid JWT token.");
                return new UnprocessableEntityResult();
            }
            if (accountService.IsAdmin(activeUserId))
            {
                try
                {
                    Account? adminAccount = await accountService.GetUserAccountAsync(activeUserId);
                    bool success = await accountService.UserDeleteAsync(adminAccount!, userId, logger);
                    if (success)
                    {
                        return new OkResult();
                    }
                    else
                    {
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> EnableUser(int userId, string token)
        {
            AccountDBOperations accountService = new AccountDBOperations(new EFAccountGateway(),
                                                                 new EFAuthorizationGateway(),
                                                                 new EFFlagGateway(),
                                                                 new EFAMRGateway(),
                                                                 new EFActiveSessionTrackerGateway());
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: get userID from token and set logger id
            logger.DefaultTimeOut = 5000;
            string strippedToken = token.Replace("\"", "");
            int activeUserId;
            if (await accountService.ValidateToken(strippedToken))
            {
                activeUserId = await accountService.GetActiveUserAsync(strippedToken);
                logger.UserID = activeUserId.ToString();
            }
            else
            {
                _ = await logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                          $"UserManagementController.AddUser: Received invalid JWT token.");
                return new UnprocessableEntityResult();
            }
            if (accountService.IsAdmin(activeUserId))
            {
                try
                {
                    Account? adminAccount = await accountService.GetUserAccountAsync(activeUserId);
                    bool success = await accountService.EnableAccountAsync(adminAccount!, userId);
                    if (success)
                    {
                        return new OkResult();
                    }
                    else
                    {
                        return new BadRequestResult();
                    }
                }
                catch(Exception ex)
                {
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DisableUser(int userId, string token)
        {
            AccountDBOperations accountService = new AccountDBOperations(new EFAccountGateway(),
                                                                 new EFAuthorizationGateway(),
                                                                 new EFFlagGateway(),
                                                                 new EFAMRGateway(),
                                                                 new EFActiveSessionTrackerGateway());
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: get userID from token and set logger id
            logger.DefaultTimeOut = 5000;
            string strippedToken = token.Replace("\"", "");
            int activeUserId;
            if (await accountService.ValidateToken(strippedToken))
            {
                activeUserId = await accountService.GetActiveUserAsync(strippedToken);
                logger.UserID = activeUserId.ToString();
            }
            else
            {
                _ = await logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                          $"UserManagementController.AddUser: Received invalid JWT token.");
                return new UnprocessableEntityResult();
            }
            if (accountService.IsAdmin(activeUserId))
            {
                try
                {
                    Account? adminAccount = await accountService.GetUserAccountAsync(activeUserId);
                    bool success = await accountService.DisableAccountAsync(adminAccount!, userId);
                    if (success)
                    {
                        return new OkResult();
                    }
                    else
                    {
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }
    }
}
