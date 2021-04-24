using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IGenerationMutator
    {
        List<Genotype> Mutate(List<Genotype> matingPool, double mutationRate);

        Genotype MutateGenotype(Genotype genotype, double mutationRate);

        Route MutateRoute(Route route, double mutationRate);
    }
}