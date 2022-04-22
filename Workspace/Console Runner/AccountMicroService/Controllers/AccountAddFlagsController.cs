using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.Logging;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountAddFlagsController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        [HttpPost]
        public async void Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;
            int userId = 0;// NEED TO GET USER ID
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();

                var ingsId = body.Split(",");
                if (ingsId[0] == "" || ingsId[0] == null)
                {
                    return;
                }

                for(int i = 0; i < ingsId.Length; i++)
                {
                    await _accountDBOperations.AddFlagToAccountAsync(userId, int.Parse(ingsId[i]), logger);
                }
            }
        }


    }
}



