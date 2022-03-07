using Microsoft.AspNetCore.Mvc;
using Console_Runner;
using Console_Runner.User_Management;

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
        try
        {
            Account account = new Account();
            account.Email = formData["email"].ToString();
            account.Password = formData["password"].ToString();
            Console.WriteLine(account.ToString());
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
