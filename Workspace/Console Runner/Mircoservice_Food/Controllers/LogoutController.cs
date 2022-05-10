using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microservice.AccountLogin.Controllers;
using Console_Runner.Logging;
using Console_Runner.AccountService.Authentication;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");
        
        [HttpPost]
        public async void Post(string token)
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            int userId = await _accountDBOperations.GetActiveUserAsync(token);
            if ((await _accountDBOperations.GetUserAccountAsync(userId)).CollectData)
            {
                logger.UserEmail = (await _accountDBOperations.GetUserAccountAsync(userId)).Email;
            }
            else
            {
                logger.UserEmail = null;
            }
            await _accountDBOperations.Logout(userId);
        }
    }
}
