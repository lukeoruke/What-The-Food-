using Microsoft.AspNetCore.Mvc;

using Console_Runner.FoodService;
using Console_Runner.Logging;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodAddController : Controller
    {
        [HttpPost]
        public async void Post()
        {
            FoodDBOperations foodDB = FoodServiceFactory.GetFoodService(FoodServiceFactory.DataStoreType.EntityFramework);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserEmail = null;
            logger.DefaultTimeOut = 5000;

            IFormCollection formData = Request.Form;

            
        }
    }
}