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
        private readonly IFitnessCalculator _fitnessCalculator;

        public GenerationRanker(
            IRouteParametersCalculator routeParametersCalculator, 
            IFitnessCalculator fitnessCalculator)
        {
            _routeParametersCalculator = routeParametersCalculator;
            _fitnessCalculator = fitnessCalculator;
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
                var fitness = _fitnessCalculator.Calculate(
                    genotype.SuitableMeetingsCount,
                    genotype.Distance, 
                    genotype.WaitingTime,
                    genotype.CountRouteFinishesAsPreferred);
                genotype.Fitness = fitness;
            }
            
            return generation
                .OrderByDescending(genotype => genotype.Fitness)
                .ToList();
        }
    }
}