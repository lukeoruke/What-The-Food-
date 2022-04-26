using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using Console_Runner.FoodService;
using Console_Runner.Logging;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetUpdatesFromBarcodeController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get(string barcode)
        {
            FoodDBOperations foodService = FoodServiceFactory.GetFoodService(FoodServiceFactory.DataStoreType.EntityFramework);
            LogService logService = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);

            // TODO: replace this with user ID from jwt token once implemented
            logService.UserID = "placeholder";
            logService.DefaultTimeOut = 5000;

            await logService.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.Business, DateTime.Now,
                $"Received request from {logService.UserID} to get all updates for food item {barcode}.");

            // JsonSerializer.Serialize only encodes base type properties, so...
            List<FoodUpdate> updates = await foodService.GetAllUpdatesForBarcodeAsync(barcode, logService);
            updates.ForEach(update => Console.WriteLine(JsonSerializer.Serialize(update, update.GetType())));
            List<string> jsonList = updates.ConvertAll<string>(update => $"{{ \"UpdateType\" :\"{update.GetType().Name}\",\"UpdateInfo\" :" + JsonSerializer.Serialize(update, update.GetType()) + " }");
            string finalString = "[ " + string.Join(",", jsonList) + " ]";
            Console.WriteLine(finalString);
            return finalString;
        }
    }
}
