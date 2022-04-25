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
        private readonly NewsDBOperations _dbOperations = new NewsDBOperations(efNews);

        [HttpPost]
        public async Task<ActionResult <int>> Get() //TODO: reserach
        {

            //gets user input from JS
            IFormCollection formData = Request.Form;
        }
    }
}
