using Microsoft.AspNetCore.Mvc;
using Console_Runner.FoodService;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodAddController : Controller
    {
        private  IFoodGateway _foodGateway = new EFFoodGateway();
        
        [HttpPost]
        public async void Post()
        {
            FoodDBOperations fm = new FoodDBOperations(_foodGateway);

            IFormCollection formData = Request.Form;

            
        }
    }
}