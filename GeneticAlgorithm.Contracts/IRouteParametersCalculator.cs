using System.Collections.Generic;
using Entities.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IRouteParametersCalculator
    {
        void CalculateParameters(Route route, List<Meeting> takenMeetings);
    }
}