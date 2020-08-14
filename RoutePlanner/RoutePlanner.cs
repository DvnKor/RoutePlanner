using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RoutePlanner.Repositories;

namespace RoutePlanner
{
    public class RoutePlanner
    {
        private readonly InMemoryOrganizationRepository inMemoryOrganizationRepository =
            new InMemoryOrganizationRepository();

        private readonly Random random = new Random();
        private const int PopulationSize = 20;
        private const int EliteSize = 4;
        private const double MutationRate = 0.01;
        private const int MaxGenerationsCount = 2000;

        private const int MaxMeetingDuration = 60;
        private const int DefaultWorkDayDurtaion = 60 * 8;
        private const int MaxRouteLength = DefaultWorkDayDurtaion / MaxGenerationsCount;

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
            var result = new List<Route>();
            foreach (var manager in organization.Managers)
            {
                var population = CreateInitialPopulation(organization, manager);
                for (int i = 0; i < MaxGenerationsCount; i++)
                {
                    population = GetNextGeneration(population);
                }

                result.Add(population[^1]);
            }

            return result;
        }

        private List<Route> GetNextGeneration(List<Route> currentGeneration)
        {
            currentGeneration.Sort((first, second) =>
                first.GetFitness() - second.GetFitness()); //todo сортировка по возрастанию
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
            if (!(random.NextDouble() < MutationRate)) return;
            var firstIndex = random.Next(0, route.Customers.Count);
            var secondIndex = random.Next(0, route.Customers.Count);

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

            var geneA = (int) Math.Round(random.NextDouble() * MaxRouteLength);
            var geneB = (int) Math.Round(random.NextDouble() * MaxRouteLength);

            var startGene = Math.Min(geneA, geneB);
            var endGene = Math.Max(geneA, geneB);

            for (var i = startGene; i < endGene; i++)
            {
                result.Add(route1.Customers[i]);
            }

            foreach (var customer in route2.Customers.Where(customer => !result.Contains(customer)))
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
                if (random.NextDouble() * 100 <= routeSelectionPercentage[i])
                {
                    result.Add(rankedRoutes[i]);
                }
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