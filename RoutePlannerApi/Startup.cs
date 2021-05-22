using System;
using System.IO;
using System.Reflection;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RoutePlannerApi.Auth;
using Storages;

namespace RoutePlannerApi
{
    public class Startup
    {
        private IWebHostEnvironment Env { get; }

        private IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Env = env;
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            // storages
            services.AddSingleton<IRoutePlannerContextFactory, RoutePlannerContextFactory>();
            services.AddSingleton<IUserStorage, UserStorage>();
            services.AddSingleton<IUserRightStorage, UserRightStorage>();
            services.AddSingleton<IClientStorage, ClientStorage>();
            services.AddSingleton<IMeetingStorage, MeetingStorage>();
            services.AddSingleton<IManagerScheduleStorage, ManagerScheduleStorage>();
            services.AddSingleton<IRouteStorage, RouteStorage>();

            services.AddSingleton<IUserContext, UserContext>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "RoutePlanner API", Version = "v1"});
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RoutePlanner API V1");
            });

            app.UseRouting();
            
            app.UseMiddleware<AuthMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
            });
        }
    }
}