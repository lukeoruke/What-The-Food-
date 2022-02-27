using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservcies.Account_Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account_LoginController : ControllerBase
    {
        [HttpGet]

        //place methods here
        public async Task<ActionResult <Account_Login>>  Get()
        {
            var user = new Account_Login();
            user.email = "something@testEmail.com";
            return Ok(user);
        }
    }
}
