using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RoutePlannerApi.Auth;
using RoutePlannerApi.Domain;
using RoutePlannerApi.Models;
using RoutePlannerApi.Repositories;
using RoutePlannerApi.Visualization;
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
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllers();

            services.AddSingleton<CustomerRepository>();
            services.AddSingleton<ManagerRepository>();
            services.AddSingleton<RouteVisualizer>();
            services.AddSingleton<RoutePlanner>();
            services.AddSingleton<RoutesRepository>();

            // storages
            services.AddSingleton<IRoutePlannerContextFactory, RoutePlannerContextFactory>();
            services.AddScoped<IUserStorage, UserStorage>();
            
            services.AddSingleton<IUserContext, UserContext>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<CustomerDto, Customer>();
                cfg.CreateMap<Coordinate, CoordinateDto>();
                cfg.CreateMap<CoordinateDto, Coordinate>();
                cfg.CreateMap<Manager, ManagerDto>();
            });
            
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

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