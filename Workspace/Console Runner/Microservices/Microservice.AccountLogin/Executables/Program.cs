using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microservices.AccountLogin.Controllers;
Console.WriteLine("FUCK");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
await app.UseOcelot();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

namespace Microservices.AccountLogin.Controllers
{
    public class Class
    {
        public void Main(String[] args)
        {
            Console.WriteLine("Main is running...");
            AccountLoginController tester = new AccountLoginController();
            var ok = tester.GetInfo();
            Console.WriteLine("Main is finished...");
        }
    }
}