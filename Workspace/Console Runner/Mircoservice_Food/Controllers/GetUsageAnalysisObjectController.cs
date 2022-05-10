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

            logger.LogListAsync();
            await logger.LogAsync(logger.UserEmail, Console_Runner.Logging.LogLevel.Info, Category.Data, DateTime.Now,
                "Received Usage Analysis Data ");
        
        }

    }
}
