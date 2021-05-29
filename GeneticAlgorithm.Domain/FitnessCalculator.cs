using GeneticAlgorithm.Contracts;

namespace GeneticAlgorithm.Domain
{
    public class FitnessCalculator : IFitnessCalculator
    {
        private const int SuitableMeetingReward = 25;
        private const int RouteFinishSameAsPreferredReward = 20;
        private const double DistanceMeterPenalty = 0.0005;
        private const double WaitingTimeMinutePenalty = 0.01;
        
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