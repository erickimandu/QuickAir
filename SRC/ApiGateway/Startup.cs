using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.DependencyInjection;
using Serilog;
using Microsoft.Extensions.HealthChecks;

namespace ApiGateway
{
    public class Startup
    {
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
             services.AddOcelot()
                     .AddConsul();
             services.AddHealthChecks(checks =>
                    {
                        checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(5));
                        checks.AddValueTaskCheck("HTTP Endpoint", () => new
                            ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
                    });
         }

  
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseHsts();
            }
             app.UseHttpsRedirection();
             app.UseOcelot().Wait();
        }
    }
}
