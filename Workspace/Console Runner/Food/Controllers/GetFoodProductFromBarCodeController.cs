using Console_Runner.FoodService;
using Console_Runner.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFoodProductFromBarCodeController : ControllerBase
    {
        private string barcode;
        public GetFoodProductFromBarCodeController()
        {
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            FoodDBOperations foodDb = FoodServiceFactory.GetFoodService(FoodServiceFactory.DataStoreType.EntityFramework);
            IFormCollection formData = Request.Form;
            barcode = formData["barcode"];
        }


        [HttpPost]
        public async Task<ActionResult<FoodItem>> GET()
        {
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // integrate userID get with JWT tokens
            logger.UserID = "how do i get this";
            FoodDBOperations foodDb = FoodServiceFactory.GetFoodService(FoodServiceFactory.DataStoreType.EntityFramework);
            return await foodDb.GetScannedItemAsync(barcode, logger);
        }
    }
}
