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
                return JsonSerializer.Serialize(loginTrend);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new StatusCodeResult(500);
            }
        
        }

    }
}
