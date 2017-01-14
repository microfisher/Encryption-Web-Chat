using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Wesley.Component.WebChat.Data;
using Microsoft.AspNetCore.Http;

namespace Wesley.Component.WebChat.Example
{
    public class Startup 
    {
        public IConfigurationRoot Configuration { get; } 

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var timeout = Configuration["AppSettings:Session:Timeout"];
            var cookieName = Configuration["AppSettings:Session:CookieName"];
            var redisConnection = Configuration["AppSettings:Caching:ConnectionString"];
            
            services.AddCors();

            services.AddOptions();

            services.AddSession(options => 
            {
                options.CookieName = cookieName;
                options.IdleTimeout = new TimeSpan(0, Convert.ToInt32(timeout), 0);
            });

            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver =new DefaultContractResolver());

            services.AddSignalR(options => {
                options.Hubs.EnableDetailedErrors = true;
            });

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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Messager/Error");
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            app.UseSession();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Messager}/{action=Login}/{id?}");
            });

            app.UseWebSockets();

            app.UseSignalR();

        }
    }
}
