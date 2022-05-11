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
    public class AccountSettingsController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");
        private int userId = -1;
        private string userName = "";
        private string userEmail = "";


        [HttpGet]
        public async Task<ActionResult<string>> Get(string token)
        {
           
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);

            userId = await _accountDBOperations.GetActiveUserAsync(token);

            userName = (await _accountDBOperations.GetUserAccountAsync(userId)).FName + " " + (await _accountDBOperations.GetUserAccountAsync(userId)).LName;
            userEmail = (await _accountDBOperations.GetUserAccountAsync(userId)).Email;

            Console.WriteLine("USER ID: " + userId.ToString());

            string jsonStr = "{\"name\":\"" + userName + "\", " + "\"email\":\"" + userEmail + "\"}";
            Console.WriteLine(jsonStr);

            return jsonStr;
        }
            

    }
}



