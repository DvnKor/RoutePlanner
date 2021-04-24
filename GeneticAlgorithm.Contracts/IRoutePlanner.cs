using System.Collections.Generic;
using Entities.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IRoutePlanner
    {
        Route[] GetBestRoutes(
            List<ManagerSchedule> managerSchedules,
            List<Meeting> meetings,
            int generationCount,
            int populationSize,
            int eliteSize,
            int mutationRate);
    }
}