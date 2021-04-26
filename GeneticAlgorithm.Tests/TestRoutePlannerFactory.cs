using GeneticAlgorithm.Domain;

namespace GeneticAlgorithm.Tests
{
    public class TestRoutePlannerFactory
    {
        public static RoutePlanner Create()
        {
            var populationGenerator = new PopulationGenerator();
            var routeParametersCalculator = new RouteParametersCalculator(
                new FitnessCalculator(), new FakeRouteStepCalculator());
            var generationRanker = new GenerationRanker(routeParametersCalculator);
            var generationCreator = new GenerationCreator(
                generationRanker,
                new GenerationSelector(),
                new GenerationBreeder(),
                new GenerationMutator());
            return new RoutePlanner(populationGenerator, generationCreator, generationRanker);
        }
    }
}