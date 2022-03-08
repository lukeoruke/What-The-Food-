using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.ScanSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanSearchController : ControllerBase
    {
        /// <summary>
        /// Get Scans for ScanSearch Microservice
        /// </summary>
        //TODO: Finish writing both the get and put
        [HttpGet]
        public async Task<ActionResult<ScanSearch>> Get()
        {
            var product = new ScanSearch();
            product.id= 123123123;
            return Ok(product);
        }
    }
}
