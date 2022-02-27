using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Scan_Search.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Scan_SearchController : ControllerBase
    {
        [HttpGet]

        //place methods here
        public async Task<ActionResult<Scan_Search>> Get()
        {
            var product = new Scan_Search();
            product.id= 123123123;
            return Ok(product);
        }
    }
}
