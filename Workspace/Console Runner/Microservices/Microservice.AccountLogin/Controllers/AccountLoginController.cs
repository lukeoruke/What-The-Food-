using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.AccountLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyAllowSpecificOrigins")]
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

        [HttpPost]
        public void Post()
        {
            Console.WriteLine("SUCCESSS!!!");
            //Console.WriteLine("Received Post from LoginController");
            ////Console.WriteLine(Request.Form("username"));

            //IFormCollection formData = Request.Form;

            //Console.WriteLine(formData["email"]);
            //Console.WriteLine(formData["password"]);
            //try
            //{
            //    Account account = new Account();
            //    account.Email = formData["email"].ToString();
            //    account.Password = formData["password"].ToString();
            //    Console.WriteLine(account.ToString());
            //}
            //catch (FileNotFoundException e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
        }
    }
}
