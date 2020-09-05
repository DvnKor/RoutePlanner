using System;
using System.Collections.Generic;
using System.Linq;
using RoutePlannerApi.Domain;
using RoutePlannerApi.Repositories;
using RoutePlannerApi.Visualization;

namespace RoutePlannerApi
{
    public class RoutePlanner
    {
        private readonly RouteVisualizer _routeVisualizer;
        private readonly ManagerRepository _managerRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly Random _random = new Random();

        private const int PopulationSize = 20;
        private const int EliteSize = 4;
        private const double MutationRate = 0.01;
        private const int MaxGenerationsCount = 10000;

        private const int MaxMeetingDuration = 60;
        private const int DefaultWorkDayDuration = 60 * 8;
        private const int MaxRouteLength = DefaultWorkDayDuration /  MaxMeetingDuration;

        private Dictionary<int, List<Customer>> _currentRoutes;

        public RoutePlanner(RouteVisualizer routeVisualizer, ManagerRepository managerRepository,
            CustomerRepository customerRepository)
        {
            _routeVisualizer = routeVisualizer;
            _managerRepository = managerRepository;
            _customerRepository = customerRepository;
        }

        public Dictionary<int, List<Customer>> GetAllCurrentRoutes()
        {
            if (_currentRoutes != null)
                return _currentRoutes;
            _currentRoutes = new Dictionary<int, List<Customer>>();
            var customers = _customerRepository.GetAllCustomers();
            var managers = _managerRepository.GetAllManagers();
            foreach (var manager in managers)
            {
                var population = CreateInitialPopulation(customers, manager);
                for (var i = 0; i < MaxGenerationsCount; i++)
                {
                    population = GetNextGeneration(population);
                }

                var bestRoute = population[0].GetRoute();
                foreach (var customer in bestRoute)
                {
                    customer.IsVisited = true;
                }
                _currentRoutes[manager.Id] = bestRoute;
            }

            _routeVisualizer.VisualizeRoutes(_currentRoutes.Values.ToList(), customers);
            return _currentRoutes;
        }


        private List<Route> GetNextGeneration(List<Route> currentGeneration)
        {
            currentGeneration.Sort((first, second) =>
                second.GetFitness() - first.GetFitness());
            var selected = GetSelectionResults(currentGeneration);
            var nextGeneration = BreedPopulation(selected);
            foreach (var route in nextGeneration)
            {
                Mutate(route);
            }

            return nextGeneration;
        }

        private void Mutate(Route route)
        {
            if (_random.NextDouble() > MutationRate) return;
            var firstIndex = _random.Next(0, MaxRouteLength);
            var secondIndex = _random.Next(0, MaxRouteLength);

            var t = route.Customers[firstIndex];
            route.Customers[firstIndex] = route.Customers[secondIndex];
            route.Customers[secondIndex] = t;
        }

        private List<Route> BreedPopulation(List<Route> routes)
        {
            var result = new List<Route>();

            for (var i = 0; i < EliteSize; i++)
                result.Add(routes[i]);
            for (var i = EliteSize; i < routes.Count; i++)
                result.Add(Breed(routes[i], routes[routes.Count - i - 1]));
            return result;
        }

        private Route Breed(Route route1, Route route2)
        {
            var result = new List<Customer>();

            var geneA = _random.Next(0, MaxRouteLength);
            var geneB = _random.Next(0, MaxRouteLength);

            var startGene = Math.Min(geneA, geneB);
            var endGene = Math.Max(geneA, geneB);

            for (var i = startGene; i < endGene; i++)
            {
                result.Add(route1.Customers[i]);
            }

            var otherCustomers = route2.Customers.Where(customer => !result.Contains(customer)).ToList();

            foreach (var customer in otherCustomers)
            {
                result.Add(customer);
            }

            return new Route(result, route1.Manager);
        }

        private List<Route> GetSelectionResults(List<Route> rankedRoutes)
        {
            var result = new List<Route>();
            var routeSelectionPercentage = GetRoutesSelectionPercentage(rankedRoutes);
            for (var i = 0; i < EliteSize; i++)
            {
                result.Add(rankedRoutes[i]);
            }

            for (var i = EliteSize; i < rankedRoutes.Count; i++)
            {
                var pick = _random.NextDouble() * 100;
                for (var j = 0; j < routeSelectionPercentage.Count; j++)
                {
                    if (pick <= routeSelectionPercentage[j])
                    {
                        result.Add(rankedRoutes[j]);
                        break;
                    }
                }
            }

            return result;
        }

        private List<double> GetRoutesSelectionPercentage(List<Route> rankedRoutes)
        {
            var result = new List<double>();
            var sum = 0;
            foreach (var route in rankedRoutes)
            {
                sum += route.GetFitness();
                result.Add(sum);
            }

            for (var i = 0; i < result.Count; i++)
            {
                result[i] = 100 * (result[i] / sum);
            }

            return result;
        }

        private List<Route> CreateInitialPopulation(List<Customer> customers, Manager manager)
        {
            var result = new List<Route>();
            for (var i = 0; i < PopulationSize; i++)
            {
                result.Add(GetRandomRoute(customers, manager));
            }

            return result;
        }

        private Route GetRandomRoute(List<Customer> customers, Manager manager)
        {
            return new Route(customers.OrderBy(arg => Guid.NewGuid()).ToList(), manager);
        }
    }
}