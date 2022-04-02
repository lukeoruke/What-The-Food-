using Console_Runner.FoodService;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFoodProductFromBarCodeController : ControllerBase
    {
        private readonly IFoodGateway _foodServiceGateway = new EFFoodGateway();
        private readonly FoodDBOperations _foodDB;
        private string barcode;
        public GetFoodProductFromBarCodeController()
        {
            _foodDB = new FoodDBOperations(_foodServiceGateway);
            IFormCollection formData = Request.Form;
            barcode = formData["barcode"];
        }


        [HttpPost]
        public async Task<ActionResult<FoodItem>> GET()
        {
            return await _foodDB.GetScannedItemAsync(barcode);
        }
    }
}
