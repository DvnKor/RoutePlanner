using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IRoutePlanner
    {
        Genotype GetBestRoutes(
            List<ManagerSchedule> managerSchedules,
            List<Meeting> meetings,
            int generationCount,
            int populationSize,
            int eliteSize,
            double mutationRate);
    }
}