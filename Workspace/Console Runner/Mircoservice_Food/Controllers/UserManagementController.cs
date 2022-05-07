using Microsoft.AspNetCore.Mvc;
using Console_Runner.Account;
using Console_Runner.AccountService;
using Console_Runner.Logging;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Account acc, string token)
        {
            AccountDBOperations accountService = new AccountDBOperations(new EFAccountGateway(),
                                                                 new EFAuthorizationGateway(),
                                                                 new EFFlagGateway(),
                                                                 new EFAMRGateway(),
                                                                 new EFActiveSessionTrackerGateway());
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: get userID from token and set logger id
            logger.DefaultTimeOut = 5000;
            var successfullyAdded = await accountService.UserSignUpAsync(acc, logger);
            if (successfullyAdded)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] Account acc, string token)
        {

        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(int userId, string token)
        {

        }

        [HttpPost]
        public async Task<IActionResult> EnableUser(int userId, string token)
        {

        }

        [HttpPost]
        public async Task<IActionResult> DisableUser(int userId, string token)
        {

        }
    }
}
