using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlightMobileWeb.Model;
using FlightMoblie.Client;
using FlightMoblie.Manager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlightMoblie
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InitContext.ip = Configuration.GetValue<string>("Logging:simulatortcp:ip");
            InitContext.port = Configuration.GetValue<string>("Logging:simulatortcp:port");
            InitContext.httpAddress = Configuration.GetValue<string>("Logging:simulatortcp:HttpAddress");
            services.AddControllers();
            // Using Singelton DP.
            services.AddSingleton(typeof(IClient), typeof(MyClient));
            services.AddSingleton(typeof(CommandManager));
            services.AddSingleton(typeof(Mutex));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
