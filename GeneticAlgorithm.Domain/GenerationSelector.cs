using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Contracts.Models;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain
{
    public class GenerationSelector : IGenerationSelector
    {
        private readonly Random _random = new Random();
        
        public List<Genotype> GetSelection(List<Genotype> rankedGeneration, int eliteSize)
        {
            var selection = new List<Genotype>();
            for (var index = 0; index < eliteSize; index++)
            {
                selection.Add(rankedGeneration[index]);
            }

            var rankedGenotypesCount = rankedGeneration.Count;
            var routeSelectionPercentage = rankedGeneration
                .Select(genotype => genotype.Fitness)
                .CumulativePercentage()
                .ToArray();
            for (var index = eliteSize; index < rankedGenotypesCount; index++)
            {
                var pick = _random.NextDouble();
                for (var j = 0; j < rankedGenotypesCount; j++)
                {
                    if (pick <= routeSelectionPercentage[j])
                    {
                        selection.Add(rankedGeneration[j]);
                        break;
                    }
                }
            }

            return selection;
        }
    }
}