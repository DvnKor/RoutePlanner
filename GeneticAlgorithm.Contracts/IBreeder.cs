using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Domain.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IBreeder
    {
        List<Genotype> Breed(List<Genotype> matingPool, int eliteSize);

        Genotype BreedGenotypes(Genotype firstGenotype, Genotype secondGenotype);

        Route BreedRoutes(Route firstRoute, Route secondRoute);
    }
}