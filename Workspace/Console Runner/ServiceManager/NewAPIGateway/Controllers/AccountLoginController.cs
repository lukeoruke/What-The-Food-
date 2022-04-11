using Microsoft.AspNetCore.Mvc;

namespace NewAPIGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountLoginController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public void Post()
    {
        Console.WriteLine("SUCCESSS!!!");
        Console.WriteLine("Received Post from LoginController");
        //Console.WriteLine(Request.Form("username"));

        IFormCollection formData = Request.Form;

        Console.WriteLine(formData["email"]);
        Console.WriteLine(formData["password"]);
    }
}

