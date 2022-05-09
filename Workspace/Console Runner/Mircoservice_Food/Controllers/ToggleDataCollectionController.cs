using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ToggleDataCollectionController
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
            int userId = await _accountDBOperations.GetActiveUserAsync(token);
            await _accountDBOperations.ToggleDataCollectionStatus(userId);
        }
    }
}
