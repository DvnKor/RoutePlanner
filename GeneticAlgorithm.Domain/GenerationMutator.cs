using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Domain
{
    public class GenerationMutator : IGenerationMutator
    {
        private readonly Random _random = new Random();
        
        public IEnumerable<Genotype> Mutate(IEnumerable<Genotype> matingPool, double mutationRate)
        {
            return matingPool.Select(genotype => MutateGenotype(genotype, mutationRate));
        }

        public Genotype MutateGenotype(Genotype genotype, double mutationRate)
        {
            var routesCount = genotype.Routes.Length;
            var mutatedRoutes = new Route[routesCount];
            for (var index = 0; index < routesCount; index++)
            {
                var route = genotype.Routes[index];
                mutatedRoutes[index] = MutateRoute(route, mutationRate);
            }

            return new Genotype(mutatedRoutes);
        }

        public Route MutateRoute(Route route, double mutationRate)
        {
            var possibleMeetingsCount = route.PossibleMeetings.Count;
            for (var index = 0; index < possibleMeetingsCount; index++)
            {
                // Мутация происходит с вероятностью mutationRate
                if (_random.NextDouble() >= mutationRate) continue;
                
                var swapWithIndex = _random.Next(0, possibleMeetingsCount);
                var first = route.PossibleMeetings[index];
                var second = route.PossibleMeetings[swapWithIndex];

                route.PossibleMeetings[index] = second;
                route.PossibleMeetings[swapWithIndex] = first;
            }

            return route;
        }
    }
}