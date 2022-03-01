using Microsoft.AspNetCore.Mvc;

namespace Front_End.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    public LoginController()
    {

    }

    [HttpPost]
    //public void Post([FromBody] string value)
    public void Post()
    {
        Console.WriteLine("Received Post from LoginController");
        //Console.WriteLine(Request.Form("username"));

        IFormCollection formData = Request.Form;

        Console.WriteLine(formData["email"]);
        Console.WriteLine(formData["password"]);
    }
}
