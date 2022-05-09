using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Console_Runner.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteUserDataController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");
        private readonly IUserIDGateway _userIDGateway = new EFUserIdentifierGateway();

        [HttpPost]
        public async void Post(string token)
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            await _accountDBOperations.DeleteAllUserData(await _accountDBOperations.GetActiveUserAsync(token));
            await _userIDGateway.RemoveUserIdAsync(token);
        }
    }
}