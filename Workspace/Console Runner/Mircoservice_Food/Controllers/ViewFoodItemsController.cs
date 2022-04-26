using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

using Console_Runner.Logging;
using Console_Runner.FoodService;
using Mircoservice_Food;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewFoodItemsController : ControllerBase
    {
        private readonly IFoodGateway _foodGateway = new EFFoodGateway();
        private readonly IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();
            
        [HttpGet]
        public async Task<ActionResult<string>> GET(int pageno)
        {
            FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway, _foodUpdateGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;
            try
            {
                var foodList = await _foodDBOperations.GetNFoodItemsAsync(1*pageno, 1, logger);
                return JsonSerializer.Serialize(foodList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new StatusCodeResult(500);
            }
        }
    }
}
