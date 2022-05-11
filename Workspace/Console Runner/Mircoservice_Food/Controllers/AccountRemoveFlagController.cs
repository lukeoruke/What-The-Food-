using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.Logging;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountRemoveFlagController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        [HttpPost]
        public async void Post(string page, string token)
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it

            logger.DefaultTimeOut = 5000;
            int userId = 0;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();

                userId = await _accountDBOperations.GetActiveUserAsync(token);

                if ((await _accountDBOperations.GetUserAccountAsync(userId)).CollectData)
                {
                    logger.UserEmail = (await _accountDBOperations.GetUserAccountAsync(userId)).Email;
                }
                else
                {
                    logger.UserEmail = null;
                }
                var ingsId = body.Split(",");
                if (ingsId[0] == "" || ingsId[0] == null)
                {
                    return;
                }


                for (int i = 0; i < ingsId.Length; i++)
                {
                    Console.WriteLine(ingsId[i]);
                    await _accountDBOperations.RemoveFoodFlagAsync(userId, int.Parse(ingsId[i]), logger);
                }
            }
        }


    }
}




