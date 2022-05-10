using Console_Runner.Account.Account_Implementation_EntityFramework;
using Console_Runner.Account.AccountInterfaces;
using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Console_Runner.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsIncrementController : Controller
    {
        private readonly IAccountNews efNews = new EFNews();
        private NewsDBOperations _dbOperations;
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");
        int userId = -1; 

        [HttpPost]
        /// <summary>
        /// Increment the Number of Health News that should be displayed
        /// </summary>
        /// <returns>Returns true is filter is incremented</returns>
        /// <exception cref="Exception">Throw if post call is unreachable</exception>
        public async Task<ActionResult<bool>> Post(string token)
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            userId = await _accountDBOperations.GetActiveUserAsync(token);

            if ((await _accountDBOperations.GetUserAccountAsync(userId)).CollectData)
            {
                logger.UserEmail = (await _accountDBOperations.GetUserAccountAsync(userId)).Email;
            }
            else
            {
                logger.UserEmail = null;
            }
            try
            {
                _dbOperations = new NewsDBOperations(efNews);
                await _dbOperations.IncrementHealthNews(userId);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("ERROR: unable to call POST method in Increment controller: " + e.Message);

            }


        }
    }
}




