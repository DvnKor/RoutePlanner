using System;

namespace RoutePlanner
{
    class Program
    {
        static void Main(string[] args)
        {
            var routePlanner = new RoutePlanner();
            var routeVisualizer = new RouteVisualizer();
            var organizationRoutes = routePlanner.GetAllCurrentRoutes();
            foreach (var organizationRoute in organizationRoutes)
            {
                var organization = organizationRoute.Key;
                var routes = organizationRoute.Value;
                routeVisualizer.VisualizeRoutes(routes, organization.Customers);

            }
            Console.WriteLine("Hello World!");
        }
    }
}
