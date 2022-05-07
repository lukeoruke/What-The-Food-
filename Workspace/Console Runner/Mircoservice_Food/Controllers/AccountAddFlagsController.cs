using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microservice.AccountLogin.Controllers;
using Console_Runner.Logging;
using Console_Runner.AccountService.Authentication;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountAddFlagsController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");
        private int userId = 0;


        [HttpPost]

        public async void Post(string token)
        {
           
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;


            
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                
                var ingsId = body.Split(",");
                if (ingsId[0] == "" || ingsId[0] == null)
                {
                    return;
                }
                string rToken = token.Split("\"")[1];
                userId = await _accountDBOperations.GetActiveUserAsync(rToken);

                Console.WriteLine("USER ID: " + userId.ToString());
                for (int i = 0; i < ingsId.Length; i++)
                {
                    await _accountDBOperations.AddFlagToAccountAsync(userId, int.Parse(ingsId[i]), logger);
                }
            }
        }


    }
}



