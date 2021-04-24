using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Domain.Models;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain
{
    public class PopulationGenerator : IPopulationGenerator
    {
        private readonly IRouteCreator _routeCreator;

        public PopulationGenerator(IRouteCreator routeCreator)
        {
            _routeCreator = routeCreator;
        }
        
        public Genotype[] CreatePopulation(
            List<ManagerSchedule> managerSchedules,
            List<Meeting> meetings, 
            int populationSize)
        {
            return Enumerable.Range(0, populationSize)
                .Select(_ => CreateGenotype(managerSchedules, meetings))
                .OrderByDescending(genotype => genotype.Fitness)
                .ToArray();
        }

        private Genotype CreateGenotype(IEnumerable<ManagerSchedule> managerSchedules, List<Meeting> meetings)
        {
            var routes = managerSchedules
                .Select(managerSchedule => CreateRoute(managerSchedule, meetings))
                .ToArray();
            return new Genotype(routes);
        }

        private Route CreateRoute(ManagerSchedule managerSchedule, List<Meeting> meetings)
        {
            var possibleMeetings = meetings.Shuffle();
            return _routeCreator.Create(managerSchedule, meetings);
        }
    }
}