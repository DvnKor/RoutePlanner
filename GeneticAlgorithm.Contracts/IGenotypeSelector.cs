using System.Collections.Generic;
using GeneticAlgorithm.Domain.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IGenotypeSelector
    {
        List<Genotype> MakeSelection(Genotype[] rankedGenotypes, int eliteSize);
    }
}