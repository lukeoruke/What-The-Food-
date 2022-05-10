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
                Console.WriteLine(loginTrend);
                Console.WriteLine(signupTrend);
                return JsonSerializer.Serialize(new
                {
                    logins = loginTrend,
                    signups = signupTrend
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
