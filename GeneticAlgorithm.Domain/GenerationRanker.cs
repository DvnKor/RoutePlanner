using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Domain
{
    public class GenerationRanker : IGenerationRanker
    {
        private readonly IRouteParametersCalculator _routeParametersCalculator;

        public GenerationRanker(IRouteParametersCalculator routeParametersCalculator)
        {
            _routeParametersCalculator = routeParametersCalculator;
        }

        /// <summary>
        /// Возвращает отсортированный в порядке убывания фитнесс функции набор генотипов
        /// </summary>
        public List<Genotype> Rank(List<Genotype> generation)
        {
            foreach (var genotype in generation)
            {
                var takenMeetings = new List<Meeting>();
                foreach (var route in genotype.Routes)
                {
                    _routeParametersCalculator.CalculateParameters(route, takenMeetings);
                    takenMeetings.AddRange(route.SuitableMeetings);
                }
            }
            
            return generation
                .OrderByDescending(genotype => genotype.Fitness)
                .ToList();
        }
    }
}