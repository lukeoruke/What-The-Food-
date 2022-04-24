using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using Console_Runner.Logging;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddAMRController : Controller
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();

        [HttpGet]
        public async Task<ActionResult<string>> GET()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;
            int userId = 0;// NEED TO GET USER ID
            var AMR = _accountDBOperations.GetAMRAsync(userId, logger);
            return JsonSerializer.Serialize(AMR);

        }
    }
}
