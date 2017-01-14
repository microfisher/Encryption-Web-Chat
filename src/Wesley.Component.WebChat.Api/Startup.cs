using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Wesley.Component.WebChat.Data;

namespace Wesley.Component.WebChat.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var cookieName = Configuration["AppSettings:Session:CookieName"];
            var timeout = Configuration["AppSettings:Session:Timeout"];
            var redisConnection = Configuration["AppSettings:Caching:ConnectionString"];

            services.AddCors();

            services.AddSession(options =>
            {
                options.CookieName = cookieName;
                options.IdleTimeout = new TimeSpan(0, Convert.ToInt32(timeout), 0);
            });

            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddSignalR(options => options.Hubs.EnableDetailedErrors = true);

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IMessageRepository, MessageRepository>();

            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = redisConnection;
            //});
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            app.UseSession();

            app.UseMvc();

            app.UseWebSockets();

            app.UseSignalR();

        }
    }
}
