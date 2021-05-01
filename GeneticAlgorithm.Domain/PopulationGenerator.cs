using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Contracts.Models;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain
{
    public class PopulationGenerator : IPopulationGenerator
    {
        public List<Genotype> CreatePopulation(
            List<ManagerSchedule> managerSchedules,
            List<Meeting> meetings, 
            int populationSize)
        {
            return Enumerable.Range(0, populationSize)
                .Select(_ => CreateGenotype(managerSchedules, meetings))
                .OrderByDescending(genotype => genotype.Fitness)
                .ToList();
        }

        private Genotype CreateGenotype(IEnumerable<ManagerSchedule> managerSchedules, List<Meeting> meetings)
        {
            var routes = managerSchedules
                .Select(managerSchedule => CreateRoute(managerSchedule, meetings))
                .ToArray();
            return new Genotype(routes);
        }

        private Route CreateRoute(ManagerSchedule managerSchedule, IList<Meeting> meetings)
        {
            var possibleMeetings = meetings.Shuffle();
            return new Route
            {
                ManagerScheduleId = managerSchedule.Id,
                ManagerSchedule = managerSchedule,
                PossibleMeetings = possibleMeetings
            };
        }
    }
}