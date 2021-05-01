using System.Collections.Generic;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IGenerationSelector
    {
        List<Genotype> GetSelection(List<Genotype> rankedGeneration, int eliteSize);
    }
}