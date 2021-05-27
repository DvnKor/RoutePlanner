using System;
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

        public IEnumerable<Genotype> GetBestRoutesProgress(
            IList<ManagerSchedule> managerSchedules,
            IList<Meeting> meetings, 
            int generationCount, 
            int populationSize,
            int eliteSize, 
            double mutationRate)
        {
            CheckParameters(generationCount, populationSize, eliteSize, mutationRate);

            var generation = _populationGenerator.CreatePopulation(
                managerSchedules, meetings, populationSize);
            var rankedGeneration = _generationRanker.Rank(generation);

            for (var generationIndex = 0; generationIndex < generationCount; generationIndex++)
            {
                generation = _generationCreator.CreateNextGeneration(rankedGeneration, eliteSize, mutationRate);
                rankedGeneration = _generationRanker.Rank(generation);
                yield return rankedGeneration.First();
            }
        }

        public Genotype GetBestRoutes(
            IList<ManagerSchedule> managerSchedules,
            IList<Meeting> meetings, 
            int generationCount, 
            int populationSize,
            int eliteSize, 
            double mutationRate)
        {
            return GetBestRoutesProgress(
                    managerSchedules,
                    meetings,
                    generationCount,
                    populationSize,
                    eliteSize,
                    mutationRate)
                .OrderByDescending(x => x.SuitableMeetingsCount)
                .First();
        }

        private static void CheckParameters(
            int generationCount, 
            int populationSize,
            int eliteSize,
            double mutationRate)
        {
            if (generationCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(generationCount));
            }

            if (populationSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(populationSize));
            }

            if (eliteSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(eliteSize));
            }

            if (mutationRate < 0 || mutationRate > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(mutationRate));
            }
        }
    }
}