using GeneticAlgorithm.Contracts;

namespace GeneticAlgorithm.Domain
{
    public class FitnessCalculator : IFitnessCalculator
    {
        private const int SuitableMeetingReward = 50;
        private const int RouteFinishSameAsPreferredReward = 200;
        
        public double Calculate(
            int suitableMeetingsCount,
            double distance, 
            double waitingTime,
            int countRouteFinishesAsPreferred)
        {
            var numerator = SuitableMeetingReward * suitableMeetingsCount +
                countRouteFinishesAsPreferred * RouteFinishSameAsPreferredReward;

            var denominator = distance / 1000 + waitingTime / 5;
            
            return denominator != 0 ? numerator / denominator : numerator;
        }
    }
}