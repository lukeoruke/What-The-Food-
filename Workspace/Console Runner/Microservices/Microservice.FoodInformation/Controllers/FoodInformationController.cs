using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.FoodInformation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodInformationController : ControllerBase
    {
        [HttpGet]

        //place methods here
        public async Task<ActionResult<FoodInformation>> Get()
        {
            var foods = new FoodInformation();
            foods.name = "Monster Energy";
            return Ok(foods);
        }
    }
}
