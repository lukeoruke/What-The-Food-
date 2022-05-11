using Console_Runner.Account.Account_Implementation_EntityFramework;
using Console_Runner.Account.AccountInterfaces;
using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : Controller
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");

        private readonly IAccountNews efNews = new EFNews();
        private NewsDBOperations _dbOperations;
        int userID = -1; //TODO: Needs JWT Token
        [HttpGet]
        /// <summary>
        /// Gets the Number of Health News that should be displayed
        /// </summary>
        /// <returns>Returns number of health news to display</returns>
        /// <exception cref="Exception">Throw if get call is unreachable</exception>
        public async Task<ActionResult<int>> Get(string token)
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);

            try
            {
                userID = await _accountDBOperations.GetActiveUserAsync(token);
                _dbOperations = new NewsDBOperations(efNews);
                return await _dbOperations.GetHealthBias(userID);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: unable to call GET method in newscontroller: " + e.Message.ToString());
                throw new Exception("ERROR: unable to call GET method in newscontroller: " + e.Message.ToString());

            }


        }
        
    }

}
