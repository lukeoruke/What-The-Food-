using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
/// <summary>
/// Add services to the container.
/// </summary>
builder.Services.AddRazorPages();

/// <summary>
/// Allows for front end to API Gateway connection and bypass CORS policy
/// </summary>
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          //allows for all origins, methods, and headers
                          builder.AllowAnyOrigin(); 
                          builder.AllowAnyMethod();
                          builder.AllowAnyHeader();
                      });
});
/// <summary>
/// ocelot implementation, accessed through ocelot.json
/// </summary>
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot();

var app = builder.Build();

/// <summary>
/// Configure the HTTP request pipeline.
/// </summary>
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//allows bypass of CORS
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();
//allows for use of ocelot
await app.UseOcelot();

app.UseAuthorization();

app.MapRazorPages();

app.Run();