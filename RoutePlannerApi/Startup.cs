using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoutePlannerApi.Repositories;
using RoutePlannerApi.Visualization;

namespace RoutePlannerApi
{
    public class Startup
    {
        private IWebHostEnvironment env { get; }
        private IConfiguration configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            this.configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var mvc = services.AddControllersWithViews();
            services.AddRazorPages();
            if (env.IsDevelopment())
                mvc.AddRazorRuntimeCompilation();
            
            services.AddScoped<CustomerRepository>();
            services.AddScoped<ManagerRepository>();
            services.AddScoped<RouteVisualizer>();
            services.AddScoped<RoutePlanner>();
            services.AddScoped<RoutesRepository>();

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
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
