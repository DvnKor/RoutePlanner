using GeneticAlgorithm.Domain;

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
                generationRanker,
                new GenerationSelector(),
                new GenerationBreeder(),
                new GenerationMutator());
            return new RoutePlanner(populationGenerator, generationCreator, generationRanker);
        }
    }
}