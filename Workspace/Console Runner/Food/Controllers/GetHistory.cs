using Console_Runner.Account.Account_Implementation_EntityFramework;
using Console_Runner.Account.AccountInterfaces;
using Console_Runner.AccountService;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetHistory : Controller
    {
        private readonly IAccountNews efNews = new EFNews();
        private AccountDBOperations _dbOperations;
        int userID = 1; //TODO: Needs JWT Token
        [HttpGet]
        /// <summary>
        /// Gets the Number of Health News that should be displayed
        /// </summary>
        /// <returns>Returns number of health news to display</returns>
        /// <exception cref="Exception">Throw if get call is unreachable</exception>
        public async Task<ActionResult<int>> Get()
        {
            try
            {
               
            }
            catch (Exception e)
            {
               Console.WriteLine(e);

            }


        }

    }

}

