using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IGenerationBreeder
    {
        IEnumerable<Genotype> Breed(List<Genotype> matingPool, int eliteSize);

        Genotype BreedGenotypes(Genotype firstGenotype, Genotype secondGenotype);

        Route BreedRoutes(Route firstRoute, Route secondRoute);
    }
}