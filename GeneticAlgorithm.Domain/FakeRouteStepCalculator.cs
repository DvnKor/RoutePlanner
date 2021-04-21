using GeneticAlgorithm.Contracts;

namespace GeneticAlgorithm.Domain
{
    public class FakeRouteStepCalculator : IRouteStepCalculator
    {
        public (double distance, double time) CalculateRouteStep()
        {
            // toDo реализовать манхеттенскую метрику, время ???
            throw new System.NotImplementedException();
        }
    }
}