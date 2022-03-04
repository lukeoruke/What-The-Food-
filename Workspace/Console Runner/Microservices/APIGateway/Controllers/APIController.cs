using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            RouteData test = new RouteData();
            return Ok();
        }
    }
}
