using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.UserUploads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUploadsController : ControllerBase
    {
        [HttpGet]

        //place methods here
        public async Task<ActionResult<UserUploads>> Get()
        {
            var picture = new UserUploads();
            picture.image = "image";
            return Ok(picture);
        }
    }
}
