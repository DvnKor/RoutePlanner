namespace GeneticAlgorithm.Contracts
{
    // toDo сделать кэширующую обертку над CalculateNextStep
    public interface IRouteStepCalculator
    {
        (double distance, double time) CalculateRouteStep();
    }
}