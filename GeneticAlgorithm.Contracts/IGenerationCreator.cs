using System.Collections.Generic;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IGenerationCreator
    {
        IEnumerable<Genotype> CreateNextGeneration(
            List<Genotype> rankedGeneration,
            int eliteSize,
            double mutationRate);
    }
}