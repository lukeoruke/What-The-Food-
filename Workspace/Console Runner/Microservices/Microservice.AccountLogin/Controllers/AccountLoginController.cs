using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.AccountLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountLoginController : ControllerBase
    {

        [HttpGet]

        //place methods here
        public async Task<ActionResult<AccountLogin>> Get()
        {
            var user = new AccountLogin();
            user.email = "something@testEmail.com";
            return Ok(user);
        }
    }
}
