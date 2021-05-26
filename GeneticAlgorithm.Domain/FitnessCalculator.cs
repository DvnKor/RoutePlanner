using GeneticAlgorithm.Contracts;

namespace GeneticAlgorithm.Domain
{
    public class FitnessCalculator : IFitnessCalculator
    {
        private const int SuitableMeetingReward = 10;
        private const int RouteFinishSameAsPreferredReward = 20;
        private const double DistanceMeterPenalty = 0.5 / 1000;
        private const int WaitingTimeMinutePenalty = 2 / 10;
        
        public double Calculate(
            int suitableMeetingsCount,
            double distance, 
            double waitingTime,
            int countRouteFinishesAsPreferred)
        {
            var fitness = 
                SuitableMeetingReward * suitableMeetingsCount +
                countRouteFinishesAsPreferred * RouteFinishSameAsPreferredReward -
                DistanceMeterPenalty * distance -
                WaitingTimeMinutePenalty * waitingTime;
            return fitness;
        }
    }
}