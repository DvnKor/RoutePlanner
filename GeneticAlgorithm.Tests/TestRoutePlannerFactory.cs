using GeneticAlgorithm.Domain;
using GeneticAlgorithm.Domain.RouteStepCalculator;

namespace GeneticAlgorithm.Tests
{
    public class TestRoutePlannerFactory
    {
        public static RoutePlanner Create()
        {
            var populationGenerator = new PopulationGenerator();
            var routeParametersCalculator = new RouteParametersCalculator(new FakeRouteStepCalculator());
            var generationRanker = new GenerationRanker(routeParametersCalculator, new FitnessCalculator());
            var generationCreator = new GenerationCreator(
                new GenerationSelector(),
                new GenerationBreeder(),
                new GenerationMutator());
            return new RoutePlanner(populationGenerator, generationCreator, generationRanker);
        }
    }
}