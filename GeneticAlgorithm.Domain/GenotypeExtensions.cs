using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Domain.Models;

namespace GeneticAlgorithm.Domain
{
    public static class GenotypeExtensions
    {
        /// <summary>
        /// Возвращает отсортированный в порядке убывания фитнесс функции набор генотипов
        /// </summary>
        public static Genotype[] Rank(this IEnumerable<Genotype> genotypes)
        {
            return genotypes
                .OrderByDescending(genotype => genotype.Fitness)
                .ToArray();
        }
    }
}