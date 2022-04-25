using Console_Runner.Account.Account_Implementation_EntityFramework;
using Console_Runner.Account.AccountInterfaces;
using Console_Runner.AccountService;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : Controller
    {
        private readonly IAccountNews efNews = new EFNews();
        private NewsDBOperations _dbOperations;
        int userID = 0; //TODO: We need JWT Token
        [HttpGet]
        public async Task<ActionResult <int>> Get() //TODO: reserach
        {
            try
            {
                _dbOperations = new NewsDBOperations(efNews);
                return await _dbOperations.GetHealthBias(userID);
            }
            catch(Exception e){
                throw new Exception("ERROR: unable to call GET method in controller: " + e.Message);

            }

            
        }
    }
}
