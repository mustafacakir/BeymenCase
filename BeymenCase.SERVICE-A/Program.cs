using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BeymenCase.SERVICE_A
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.ConfigureKestrel(serverOptions =>
               {
                   serverOptions.ListenAnyIP(4000);
               });
               webBuilder.UseStartup<Startup>();
           });
    }
}
