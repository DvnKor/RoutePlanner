using GeneticAlgorithm.Contracts;

namespace GeneticAlgorithm.Domain
{
    public class FitnessCalculator : IFitnessCalculator
    {
        public double Calculate(int suitableMeetingsCount, double distance, double waitingTime)
        {
            return 50 * suitableMeetingsCount / (distance / 1000 + waitingTime / 5);
        }
    }
}