using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Domain.Models;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain
{
    public class GenotypeSelector : IGenotypeSelector
    {
        private readonly Random _random = new Random();
        
        public List<Genotype> MakeSelection(Genotype[] rankedGenotypes, int eliteSize)
        {
            var selection = new List<Genotype>();
            for (var index = 0; index < eliteSize; index++)
            {
                selection.Add(rankedGenotypes[index]);
            }

            var rankedGenotypesCount = rankedGenotypes.Length;
            var routeSelectionPercentage = rankedGenotypes
                .Select(genotype => genotype.Fitness)
                .CumulativePercentage()
                .ToArray();
            for (var index = eliteSize; index < rankedGenotypesCount; index++)
            {
                var pick = _random.NextDouble() * 100;
                for (var j = 0; j < rankedGenotypesCount; j++)
                {
                    if (pick <= routeSelectionPercentage[j])
                    {
                        selection.Add(rankedGenotypes[j]);
                        break;
                    }
                }
            }

            return selection;
        }
    }
}