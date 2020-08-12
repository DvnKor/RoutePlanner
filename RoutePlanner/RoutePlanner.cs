using System;
using System.Collections.Generic;
using System.Linq;
using RoutePlanner.Repositories;

namespace RoutePlanner
{
    public class RoutePlanner
    {
        private readonly InMemoryOrganizationRepository inMemoryOrganizationRepository =
            new InMemoryOrganizationRepository();

        private const int PopulationSize = 20;
        private const int EliteSize = 4;
        private const double MutationRate = 0.01;
        private const int MaxGenerationsCount = 2000;
        public Dictionary<Organization, List<Route>> GetAllCurrentRoutes()
        {
            var result = new Dictionary<Organization, List<Route>>();
            var organizations = inMemoryOrganizationRepository.GetAllOrganizations();
            foreach (var organization in organizations)
            {
                result[organization] = GetOrganizationRoutes(organization);
            }

            return result;
        }


        private List<Route> GetOrganizationRoutes(Organization organization)
        {

            foreach (var manager in organization.Managers)
            {
                var population = CreateInitialPopulation(organization, manager);
                population.Sort((first,second)=> first.GetFitness() - second.GetFitness()); //todo сортировка по возрастанию
            }
        }

        private List<Route> GetSelectionResults(List<Route> rankedRoutes)
        {
            var result = new List<Route>();
            var random = new Random();
            var routeSelectionPercentage = GetRoutesSelectionPercentage(rankedRoutes);
            for (var i = 0; i < EliteSize; i++)
            {
                result.Add(rankedRoutes[i]);
            }
            for (var i = EliteSize; i < rankedRoutes.Count; i++)
            {
                if (random.NextDouble() * 100 <= routeSelectionPercentage[i])
                {
                    result.Add(rankedRoutes[i]);
                };
            }

            return result;
        }

        private List<int> GetRoutesSelectionPercentage(List<Route> rankedRoutes)
        {
            var result = new List<int>();
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

        private List<Route> CreateInitialPopulation(Organization organization, Manager manager)
        {
            var result = new List<Route>();
            for (var i = 0; i < PopulationSize; i++)
            {
                result.Add(GetRandomRoute(organization, manager));
            }

            return result;
        }

        private Route GetRandomRoute(Organization organization, Manager manager)
        {
            var customers = organization.Customers.ToList();
            return new Route(customers.OrderBy(arg => Guid.NewGuid()).ToList(), manager);
        }
    }
}