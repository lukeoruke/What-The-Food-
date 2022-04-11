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
            LogService logger = LogServiceLocator.GetLogService(LogServiceLocator.DataStoreType.EntityFramework);
            FoodDBOperations foodDb = FoodServiceLocator.GetFoodService(FoodServiceLocator.DataStoreType.EntityFramework);
            IFormCollection formData = Request.Form;
            barcode = formData["barcode"];
        }


        [HttpPost]
        public async Task<ActionResult<FoodItem>> GET()
        {
            LogService logger = LogServiceLocator.GetLogService(LogServiceLocator.DataStoreType.EntityFramework);
            FoodDBOperations foodDb = FoodServiceLocator.GetFoodService(FoodServiceLocator.DataStoreType.EntityFramework);
            return await foodDb.GetScannedItemAsync(barcode);
        }

        private void InstantiateServices(out FoodDBOperations foodDb)
        {
        }
    }
}
