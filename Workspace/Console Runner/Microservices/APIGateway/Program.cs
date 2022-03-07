using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Host.ConfigureAppConfiguration((hostingContext, configuration) =>
//{
//    configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//});

//Host.CreateDefaultBuilder(args)
//.ConfigureHostConfiguration(hostConfig =>
//{
//    hostConfig.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//});
//Host.CreateDefaultBuilder(args)
//    .ConfigureServices((hostContext, services) =>
//    {
//        services.AddOcelot();
//    });
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin();

                      });
});


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


app.UseCors(options =>
{
    options.
    AllowAnyOrigin().
    AllowAnyMethod().
    AllowAnyHeader();
});
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();
await app.UseOcelot();

app.UseAuthorization();

app.MapRazorPages();

app.Run();