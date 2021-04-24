using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Contracts.Models;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain
{
    public class GenerationBreeder : IGenerationBreeder
    {
        private readonly Random _random = new Random();

        public List<Genotype> Breed(List<Genotype> genotypes, int eliteSize)
        {
            var children = new List<Genotype>();
            for (var index = 0; index < eliteSize; index++)
            {
                children.Add(genotypes[index]);
            }

            var matingPool = genotypes.Shuffle();
            var matingPoolLength = matingPool.Count;
            for (var index = 0; index < matingPoolLength - eliteSize; index++)
            {
                var firstParent = matingPool[index];
                var secondParent = matingPool[matingPoolLength - (index + 1)];
                var child = BreedGenotypes(firstParent, secondParent);
                children.Add(child);
            }

            return children;
        }

        public Genotype BreedGenotypes(Genotype firstGenotype, Genotype secondGenotype)
        {
            // количество маршрутов должно быть равным в обоих генотипах
            var routesCount = firstGenotype.Routes.Length;
            
            var routesBreedResult = new Route[routesCount];
            for (var index = 0; index < routesCount; index++)
            {
                var firstRoute = firstGenotype.Routes[index];
                var secondRoute = secondGenotype.Routes[index];
                routesBreedResult[index] = BreedRoutes(firstRoute, secondRoute);
            }

            return new Genotype(routesBreedResult);
        }

        public Route BreedRoutes(Route firstRoute, Route secondRoute)
        {
            // менеджер и количество возможных встреч предполагаются равными для двух маршрутов
            var managerSchedule = firstRoute.ManagerSchedule;
            var possibleMeetingsCount = firstRoute.PossibleMeetings.Count;
            
            var breedMeetingsResult = new Meeting[possibleMeetingsCount];

            var geneA = _random.Next(0, possibleMeetingsCount);
            var geneB = _random.Next(0, possibleMeetingsCount);

            var startGene = Math.Min(geneA, geneB);
            var endGene = Math.Max(geneA, geneB);

            for (var index = startGene; index < endGene; index++)
            {
                breedMeetingsResult[index] = firstRoute.PossibleMeetings[index];
            }

            var otherMeetings = secondRoute.PossibleMeetings
                .Where(customer => !breedMeetingsResult.Contains(customer))
                .ToList();

            var indexInResult = 0;
            foreach (var meeting in otherMeetings)
            {
                while (breedMeetingsResult[indexInResult] != null)
                {
                    indexInResult++;
                }
                breedMeetingsResult[indexInResult++] = meeting;
            }

            return new Route
            {
                ManagerScheduleId = managerSchedule.Id,
                ManagerSchedule = managerSchedule,
                PossibleMeetings = breedMeetingsResult
            };
        }
    }
}