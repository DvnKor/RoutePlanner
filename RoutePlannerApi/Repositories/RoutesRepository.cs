using System.Collections.Generic;
using RoutePlannerApi.Domain;
using RoutePlannerApi.Visualization;

namespace RoutePlannerApi.Repositories
{
    public class RoutesRepository
    {
        private readonly CustomerRepository _customerRepository;
        private readonly RoutePlanner _routePlanner;
        private readonly Dictionary<int, List<Customer>> _routes;

        public RoutesRepository(CustomerRepository customerRepository, RoutePlanner routePlanner)
        {
            _customerRepository = customerRepository;
            _routePlanner = routePlanner;
            _routes = routePlanner.GetAllCurrentRoutes();
        }

        public List<Customer> GetManagerRoute(int managerId)
        {
            return _routes[managerId];
        }

        public Dictionary<int, List<Customer>> GetAllRoutes()
        {
            return _routes;
        }
    }
}