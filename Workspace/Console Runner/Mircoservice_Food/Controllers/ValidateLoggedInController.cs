using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Mircoservice_Food.Controllers
{
    public class ValidateLoggedInController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");
        private int userId = 0;

        [HttpGet]
        public async Task<ActionResult<string>> Get(string token)
        {
            Console.WriteLine("In validate login controller");
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            string rToken = token.Split("\"")[1];

            string isValid = "{isValid: "+(await _accountDBOperations.ValidateToken(rToken)).ToString()+"}";
            return isValid;
        }
    }
}





