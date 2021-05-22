using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IRoutePlanner
    {
        Genotype GetBestRoutes(
            IList<ManagerSchedule> managerSchedules,
            IList<Meeting> meetings,
            int generationCount,
            int populationSize,
            int eliteSize,
            double mutationRate);
    }
}