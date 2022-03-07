using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.UserUploads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUploadsController : ControllerBase
    {
        /// <summary>
        /// Get upload info for User Upload Microservice
        /// </summary>
        //TODO: Finish writing both the get and put
        [HttpGet]
        public async Task<ActionResult<UserUploads>> Get()
        {
            var picture = new UserUploads();
            picture.image = "image";
            return Ok(picture);
        }
    }
}
