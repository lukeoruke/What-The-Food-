using Console_Runner.FoodService;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFoodProductFromBarCodeController : ControllerBase
    {
        private readonly IFoodGateway _foodServiceGateway = new EFFoodGateway();
        private  FoodDBOperations _foodDB;
        private string barcode;

        [HttpPost]
        public async void Post()
        {
            _foodDB = new FoodDBOperations(_foodServiceGateway);

            barcode = Request.Form["barcode"];
            //barcode = Request.Form["barcode"];
        }


        [HttpGet]
        public async Task<ActionResult<FoodItem>> GET()
        {
            barcode = Request.Form["barcode"];
            Console.WriteLine(barcode);
            _foodDB = new FoodDBOperations(_foodServiceGateway);
            return await _foodDB.GetScannedItemAsync(barcode);
        }
    }
}
