using System.Collections.Generic;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Domain
{
    public class GenerationCreator : IGenerationCreator
    {
        private readonly IGenerationSelector _generationSelector;
        private readonly IGenerationBreeder _generationBreeder;
        private readonly IGenerationMutator _generationMutator;

        public GenerationCreator( 
            IGenerationSelector generationSelector,
            IGenerationBreeder generationBreeder,
            IGenerationMutator generationMutator)
        {
            _generationSelector = generationSelector;
            _generationBreeder = generationBreeder;
            _generationMutator = generationMutator;
        }

        public List<Genotype> CreateNextGeneration(
            List<Genotype> rankedGeneration,
            int eliteSize,
            double mutationRate)
        {
            var matingPool = _generationSelector.GetSelection(rankedGeneration, eliteSize);
            var children = _generationBreeder.Breed(matingPool, eliteSize);
            var nextGeneration = _generationMutator.Mutate(children, mutationRate);
            return nextGeneration;
        }
    }
}