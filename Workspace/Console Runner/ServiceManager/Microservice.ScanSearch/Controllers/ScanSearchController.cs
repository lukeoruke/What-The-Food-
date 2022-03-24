using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.ScanSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanSearchController : ControllerBase
    {
        [HttpGet]

        //place methods here
        public async Task<ActionResult<ScanSearch>> Get()
        {
            var product = new ScanSearch();
            product.id= 123123123;
            return Ok(product);
        }
    }
}
