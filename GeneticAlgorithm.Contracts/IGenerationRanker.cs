using System.Collections.Generic;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IGenerationRanker
    {
        List<Genotype> Rank(List<Genotype> generation);
    }
}