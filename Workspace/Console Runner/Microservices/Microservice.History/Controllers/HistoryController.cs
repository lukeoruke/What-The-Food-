using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.History.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        /// <summary>
        /// Get History for HistoryController Microservice
        /// </summary>
        //TODO: Finish writing both the get and put
        [HttpGet]
        public async Task<ActionResult<History>> Get()
        {
            var name = new History();
            name.name = "King FrancisIII ";
            return Ok(name);
        }
    }
}
