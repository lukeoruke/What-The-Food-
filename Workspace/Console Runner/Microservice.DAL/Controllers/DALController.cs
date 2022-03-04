using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DALController : ControllerBase
    {
        [HttpGet]
        //place methods here
        public async Task<ActionResult<DAL>> Get()
        {
            var name = new DAL();
            name.name = "bleen";
            return Ok(name);
        }
    }
}
