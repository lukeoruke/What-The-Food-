using Front_End;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    }
}
public class Program
{
    static void Main(string[] args)
    {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
    }
}
