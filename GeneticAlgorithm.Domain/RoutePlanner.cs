using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Domain
{
    public class RoutePlanner : IRoutePlanner
    {
        private readonly IPopulationGenerator _populationGenerator;
        private readonly IGenerationCreator _generationCreator;
        private readonly IGenerationRanker _generationRanker;

        public RoutePlanner(
            IPopulationGenerator populationGenerator, 
            IGenerationCreator generationCreator, 
            IGenerationRanker generationRanker)
        {
            _populationGenerator = populationGenerator;
            _generationCreator = generationCreator;
            _generationRanker = generationRanker;
        }

        public Genotype GetBestRoutes(
            IList<ManagerSchedule> managerSchedules,
            IList<Meeting> meetings, 
            int generationCount, 
            int populationSize,
            int eliteSize, 
            double mutationRate)
        {
            var generation = _populationGenerator.CreatePopulation(
                managerSchedules, meetings, populationSize);

            var progress = new List<Genotype>();
            for (var generationIndex = 0; generationIndex < generationCount; generationIndex++)
            {
                generation = _generationCreator.CreateNextGeneration(generation, eliteSize, mutationRate);
                var rankedGeneration = _generationRanker.Rank(generation);
                progress.Add(rankedGeneration.First());
            }

            var bestRoutes = progress.Last();
            return bestRoutes;
        }
    }
}