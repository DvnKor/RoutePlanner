using System.Collections.Generic;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Domain
{
    public class GenerationCreator : IGenerationCreator
    {
        private readonly IGenerationRanker _generationRanker;
        private readonly IGenerationSelector _generationSelector;
        private readonly IGenerationBreeder _generationBreeder;
        private readonly IGenerationMutator _generationMutator;

        public GenerationCreator(
            IGenerationRanker generationRanker, 
            IGenerationSelector generationSelector,
            IGenerationBreeder generationBreeder,
            IGenerationMutator generationMutator)
        {
            _generationRanker = generationRanker;
            _generationSelector = generationSelector;
            _generationBreeder = generationBreeder;
            _generationMutator = generationMutator;
        }

        public List<Genotype> CreateNextGeneration(
            List<Genotype> generation,
            int eliteSize,
            double mutationRate)
        {
            var rankedGeneration = _generationRanker.Rank(generation);
            var matingPool = _generationSelector.GetSelection(rankedGeneration, eliteSize);
            var children = _generationBreeder.Breed(matingPool, eliteSize);
            var nextGeneration = _generationMutator.Mutate(children, mutationRate);
            return nextGeneration;
        }
    }
}