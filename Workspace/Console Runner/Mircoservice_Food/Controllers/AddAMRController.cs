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

        [HttpPost]
        public async void Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;
            int userId = 0;// NEED TO GET USER ID
            var AMR = _accountDBOperations.GetAMRAsync(userId, logger);
            JsonSerializer.Serialize(AMR);

            IFormCollection formData = Request.Form;

            Console.WriteLine(formData["gender"]);
            Console.WriteLine(formData["weight"]);
            Console.WriteLine(formData["age"]);
            Console.WriteLine(formData["height"]);
            Console.WriteLine(formData["activity"]);




        }
    }
}
