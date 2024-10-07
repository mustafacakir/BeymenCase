using BeymenCase.Core.Entities;
using BeymenCase.Core.Interfaces;
using BeymenCase.Infrastructure.Configuration;
using BeymenCase.Infrastructure.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BeymenCase.WebApp
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
            services.AddControllersWithViews();

            var redisConnectionString = Configuration.GetSection("RedisSettings:ConnectionString").Value;
            services.AddSingleton<IBaseRepository<ConfigurationRecord>>(provider => new ConfigurationRepository(redisConnectionString));

            var rabbitMqSettings = Configuration.GetSection("RabbitMq");
            services.AddSingleton<IQueueService>(provider =>
                new RabbitMqService(rabbitMqSettings["HostName"], rabbitMqSettings["QueueName"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Configuration}/{action=Index}/{id?}");
            });
        }
    }
}
