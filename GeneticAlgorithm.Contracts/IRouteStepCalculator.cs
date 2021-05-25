using System;
using Infrastructure.Common;

namespace GeneticAlgorithm.Contracts
{
    public interface IRouteStepCalculator
    {
        (double distance, double time) CalculateRouteStep(
            Coordinate from, Coordinate to, DateTime departureTime);
    }
}