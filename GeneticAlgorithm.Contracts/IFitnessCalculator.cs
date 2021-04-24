namespace GeneticAlgorithm.Contracts
{
    public interface IFitnessCalculator
    {
        double Calculate(
            int suitableMeetingsCount,
            double distance, 
            double waitingTime,
            bool routeFinishesAsPreferred);
    }
}