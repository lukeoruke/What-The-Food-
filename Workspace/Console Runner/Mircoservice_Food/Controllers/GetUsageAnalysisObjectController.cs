using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using Console_Runner.AccountService;
using Console_Runner.Logging;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetUsageAnalysisObjectController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get(string token) {
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            AccountDBOperations accountService = new AccountDBOperations(new EFAccountGateway(), new EFAuthorizationGateway(), new EFFlagGateway(), new EFAMRGateway(), new EFActiveSessionTrackerGateway());

            if (await accountService.ValidateToken(token))
            {
                int activeUserId = await accountService.GetActiveUserAsync(token);
                Account? activeUser = await accountService.GetUserAccountAsync(activeUserId);
                if(activeUser?.CollectData ?? false)
                {
                    logger.UserEmail = activeUser!.Email;
                }
                else
                {
                    logger.UserEmail = "Unknown";
                }
                if (accountService.IsAdmin(activeUserId))
                {
                    try
                    {
                        var loginTrend = logger.GetLoginTrends(DateTime.Now.AddMonths(-3));
                        var signupTrend = logger.GetSignupTrends(DateTime.Now.AddMonths(-3));
                        var mostViewedPages = logger.GetMostViewedPages();
                        var highestAverageDurationPages = logger.GetHighestAverageDurationPages();
                        var mostScannedBarcodes = logger.GetMostScannedBarcodes();
                        var mostFlaggedIngredients = logger.GetMostFlaggedIngredients();
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                                   $"User requested usage analysis metrics.");
                        return JsonSerializer.Serialize(new
                        {
                            logins = loginTrend,
                            signups = signupTrend,
                            mostViewedPages = mostViewedPages,
                            highestAverageDurationPages = highestAverageDurationPages,
                            mostScannedBarcodes = mostScannedBarcodes,
                            mostFlaggedIngredients = mostFlaggedIngredients
                        });
                    }
                    catch (Exception ex)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Error, Category.Business, DateTime.Now,
                                                   $"User requested usage analysis metrics. Unknown error: {ex.Message}");
                        return new StatusCodeResult(500);
                    }
                }
                else
                {
                    _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                               $"User {activeUserId} is unauthorized to request usage analysis metrics.");
                    return new UnauthorizedResult();
                }
            }
            else
            {
                _ = logger.LogAsync("Unknown", Console_Runner.Logging.LogLevel.Debug, Console_Runner.Logging.Category.Server, DateTime.Now,
                                    $"Received invalid JWT token.");
                return new UnprocessableEntityResult();
            }
        
        }

    }
}
