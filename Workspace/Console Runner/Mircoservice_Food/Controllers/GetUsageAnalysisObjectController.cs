using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using Console_Runner.Logging;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetUsageAnalysisObjectController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get() {
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);

            try
            {
                var loginTrend = logger.GetLoginTrends(DateTime.Now.AddMonths(-3));
                var signupTrend = logger.GetSignupTrends(DateTime.Now.AddMonths(-3));
                var mostViewedPages = logger.GetMostViewedPages();
                var highestAverageDurationPages = logger.GetHighestAverageDurationPages();
                var mostScannedBarcodes = logger.GetMostScannedBarcodes();
                return JsonSerializer.Serialize(new
                {
                    logins = loginTrend,
                    signups = signupTrend,
                    mostViewedPages = mostViewedPages,
                    highestAverageDurationPages = highestAverageDurationPages,
                    mostScannedBarcodes = mostScannedBarcodes
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new StatusCodeResult(500);
            }
        
        }

    }
}
