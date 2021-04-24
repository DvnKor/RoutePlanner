using System.Collections.Generic;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IGenerationCreator
    {
        List<Genotype> CreateNextGeneration(
            List<Genotype> generation,
            int eliteSize,
            double mutationRate);
    }
}