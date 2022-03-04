using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.AccountLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountLoginController : ControllerBase
    {

        [HttpGet]
        //place methods here
        public async Task<ActionResult<AccountLogin>> GetInfo()
        {
            Console.WriteLine("Succesfully called function");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:49201/api/DAL");
            Console.WriteLine(request.Content);
            return Ok();
        }
    }
}
