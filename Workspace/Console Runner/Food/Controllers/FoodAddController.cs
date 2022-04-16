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
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            FoodDBOperations foodDB = FoodServiceFactory.GetFoodService(FoodServiceFactory.DataStoreType.EntityFramework);

            IFormCollection formData = Request.Form;

            
        }
    }
}