using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Host.ConfigureAppConfiguration((hostingContext,configuration) =>
//{
//    configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//});

//builder.Host.ConfigureServices(webBuilder =>
//{
//    webBuilder.ConfigureAppConfiguration(
//        c => c.AddJsonFile(Path.Combine("Ocelot","ocelot.json")));
//});

//builder.Host.ConfigureServices(s =>
//{
//    s.AddOcelot();
//});
//builder.Services.AddOcelot();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();