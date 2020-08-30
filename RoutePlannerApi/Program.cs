using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoutePlanner;
using RoutePlanner.Repositories;

namespace RoutePlannerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var customersRepository = new CustomerRepository();
            var routePlanner = new RoutePlanner.RoutePlanner();
            var routeVisualizer = new RouteVisualizer();
            var routes = routePlanner.GetAllCurrentRoutes();
            routeVisualizer.VisualizeRoutes(routes, customersRepository.GetAllCustomers());
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
