using System;
using RoutePlanner.Repositories;

namespace RoutePlanner
{
    class Program
    {
        static void Main(string[] args)
        {
            var customersRepository = new CustomerRepository();
            var routePlanner = new RoutePlanner();
            var routeVisualizer = new RouteVisualizer();
            var routes = routePlanner.GetAllCurrentRoutes();
            routeVisualizer.VisualizeRoutes(routes, customersRepository.GetAllCustomers());
            Console.WriteLine("Hello World!");
        }
    }
}
