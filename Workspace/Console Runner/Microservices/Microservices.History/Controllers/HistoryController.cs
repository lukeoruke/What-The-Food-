using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.History.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        [HttpGet]

        //place methods here
        public async Task<ActionResult<History>> Get()
        {
            var name = new History();
            name.name = "King FrancisIII ";
            return Ok(name);
        }
    }
}
