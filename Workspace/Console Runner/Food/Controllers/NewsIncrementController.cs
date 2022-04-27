using Console_Runner.Account.Account_Implementation_EntityFramework;
using Console_Runner.Account.AccountInterfaces;
using Console_Runner.AccountService;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsIncrementController : Controller
    {
        private readonly IAccountNews efNews = new EFNews();
        private NewsDBOperations _dbOperations;
        int userID = 1; //TODO: We need JWT Token
        [HttpPost]
        /// <summary>
        /// Increment the Number of Health News that should be displayed
        /// </summary>
        /// <returns>Returns true is filter is incremented</returns>
        /// <exception cref="Exception">Throw if post call is unreachable</exception>
        public async Task<ActionResult<bool>> Post()
        {
            try
            {
                _dbOperations = new NewsDBOperations(efNews);
                await _dbOperations.IncrementHealthNews(userID);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("ERROR: unable to call POST method in Increment controller: " + e.Message);

            }


        }
    }
}




