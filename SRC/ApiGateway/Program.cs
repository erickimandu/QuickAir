using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.DependencyInjection;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.HealthChecks;
using System.Threading.Tasks;


namespace ApiGateway
{
    public class Program
    {
        private static IConfiguration Configuration;
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
              WebHost.CreateDefaultBuilder(args)
                     .UseSerilog()
                     .UseHealthChecks("/hc")
                     .UseIISIntegration()
                     .UseKestrel()
                     .UseContentRoot(Directory.GetCurrentDirectory())
                     .ConfigureAppConfiguration((hostingContext, config) =>
                     {
                         config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                               .AddJsonFile("appsettings.json", true, true)
                               .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);

                               // add ocelot configuration
                               string ocelotConfigPath = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "OcelotConfig");
                               ocelotConfigPath = Path.Combine(ocelotConfigPath, hostingContext.HostingEnvironment.EnvironmentName);
                               config.AddOcelot(ocelotConfigPath, hostingContext.HostingEnvironment);
                               config.AddEnvironmentVariables();

                               Configuration = config.Build();
                     })
                     .ConfigureLogging((hostingContext, logging) =>
                     {
                         Log.Logger = new LoggerConfiguration()
                                                 .ReadFrom.Configuration(Configuration)
                                                 .CreateLogger();
                     })
                     .UseStartup<Startup>();
    }
}
