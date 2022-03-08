using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.FoodInformation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodInformationController : ControllerBase
    {
        /// <summary>
        /// Get Function for FoodInformation Microservice
        /// </summary>
        //TODO: Finish writing both the get and put
        [HttpGet]
        public async Task<ActionResult<FoodInformation>> Get()
        {
            var foods = new FoodInformation();
            foods.name = "Monster Energy";
            return Ok(foods);
        }
    }
}
