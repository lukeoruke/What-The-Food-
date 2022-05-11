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
                Account? activeUser = await accountService.GetUserAccountAsync(activeUserId);
                if (activeUser?.CollectData ?? false)
                {
                    logger.UserEmail = activeUser!.Email;
                }
                else
                {
                    logger.UserEmail = null;
                }
            }
            else
            {
                _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                          $"Received invalid JWT token.");
                return new UnprocessableEntityResult();
            }
            if (accountService.IsAdmin(activeUserId))
            {
                try
                {
                    var successfullyAdded = await accountService.UserSignUpAsync(acc, logger);
                    if (successfullyAdded)
                    {
                        if(logger.UserEmail != null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                           $"Admin {activeUserId} created new account.");
                        }
                        return new OkResult();
                    }
                    else
                    {
                        if(logger.UserEmail != null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                           $"Admin {activeUserId} failed to create new account. Bad arguments.");
                        }
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    if(logger.UserEmail != null)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                       $"Admin {activeUserId} could not create account. Unknown error {ex.Message}");
                    }
                    return StatusCode(500);
                }
            }
            else
            {
                if(logger.UserEmail != null)
                {
                    _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                   $"User {activeUserId} is unauthorized to create accounts.");
                }
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
                Account? activeUser = await accountService.GetUserAccountAsync(activeUserId);
                if (activeUser?.CollectData ?? false)
                {
                    logger.UserEmail = activeUser!.Email;
                }
                else
                {
                    logger.UserEmail = null;
                }
            }
            else
            {
                _ = await logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                          $"Received invalid JWT token.");
                return new UnprocessableEntityResult();
            }
            if (accountService.IsAdmin(activeUserId))
            {
                Account? adminAccount = await accountService.GetUserAccountAsync(activeUserId);
                bool success = await accountService.UserUpdateDataAsync(adminAccount!, acc.UserID, acc.FName, acc.LName, acc.Password, logger);
                if (success)
                {
                    if(logger.UserEmail != null)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"Admin {activeUserId} updated account {acc.UserID}.");
                    }
                    return new OkResult();
                }
                else
                {
                    if(logger.UserEmail != null)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                       $"Admin {activeUserId} could not update account {acc.UserID}. Bad arguments.");
                    }
                    return new BadRequestResult();
                }
            }
            else
            {
                if(logger.UserEmail != null)
                {
                    _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                   $"User {activeUserId} is not authorized to update account {acc.UserID}.");
                }
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
                Account? activeUser = await accountService.GetUserAccountAsync(activeUserId);
                if (activeUser?.CollectData ?? false)
                {
                    logger.UserEmail = activeUser!.Email;
                }
                else
                {
                    logger.UserEmail = null;
                }
            }
            else
            {
                _ = await logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                          $"Received invalid JWT token.");
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
                        if (logger.UserEmail != null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                           $"Admin {activeUserId} deleted account {userId}.");
                        }
                        return new OkResult();
                    }
                    else
                    {
                        if (logger.UserEmail != null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                           $"Admin {activeUserId} could not delete account {userId}. Bad arguments.");
                        }
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    if (logger.UserEmail != null)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"Admin {activeUserId} could not delete account {userId}. Unknown error {ex.Message}");
                    }
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                if (logger.UserEmail != null)
                {
                    _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                   $"User {activeUserId} is not authorized to delete account {userId}.");
                }
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
                Account? activeUser = await accountService.GetUserAccountAsync(activeUserId);
                if (activeUser?.CollectData ?? false)
                {
                    logger.UserEmail = activeUser!.Email;
                }
                else
                {
                    logger.UserEmail = null;
                }
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
                        if (logger.UserEmail != null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                           $"Admin {activeUserId} enabled account {userId}.");
                        }
                        return new OkResult();
                    }
                    else
                    {
                        if (logger.UserEmail != null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                           $"Admin {activeUserId} could not enable account {userId}. Bad arguments.");
                        }
                        return new BadRequestResult();
                    }
                }
                catch(Exception ex)
                {
                    if (logger.UserEmail != null)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                       $"Admin {activeUserId} could not enable account {userId}. Unknown error {ex.Message}");
                    }
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                if (logger.UserEmail != null)
                {
                    _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                   $"User {activeUserId} is not authorized to enable account {userId}.");
                }
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
                Account? activeUser = await accountService.GetUserAccountAsync(activeUserId);
                if (activeUser?.CollectData ?? false)
                {
                    logger.UserEmail = activeUser!.Email;
                }
                else
                {
                    logger.UserEmail = null;
                }
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
                        if (logger.UserEmail != null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                           $"Admin {activeUserId} disabled account {userId}.");
                        }
                        return new OkResult();
                    }
                    else
                    {
                        if (logger.UserEmail != null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                           $"Admin {activeUserId} could not disable account {userId}. Bad arguments.");
                        }
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    if (logger.UserEmail != null)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                       $"Admin {activeUserId} could not disable account {userId}. Unknown error {ex.Message}");
                    }
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                if (logger.UserEmail != null)
                {
                    _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                   $"User {activeUserId} is not authorized to disable account {userId}.");
                }
                return new UnauthorizedResult();
            }
        }
    }
}
