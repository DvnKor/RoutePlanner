using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IRoutePlanner
    {
        IEnumerable<Genotype> GetBestRoutesProgress(
            IList<ManagerSchedule> managerSchedules,
            IList<Meeting> meetings,
            int generationCount,
            int populationSize,
            int eliteSize,
            double mutationRate);
        
        IEnumerable<Genotype> GetBestRoutesProgress(
            IList<ManagerSchedule> managerSchedules,
            IList<Meeting> meetings);

        Genotype GetBestRoutes(
            IList<ManagerSchedule> managerSchedules,
            IList<Meeting> meetings,
            int generationCount,
            int populationSize,
            int eliteSize,
            double mutationRate);
        
        Genotype GetBestRoutes(
            IList<ManagerSchedule> managerSchedules,
            IList<Meeting> meetings);
    }
}