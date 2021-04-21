using System.Collections.Generic;
using Entities.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IRouteCreator
    {
        Route Create(ManagerSchedule managerSchedule, List<Meeting> possibleMeetings);
    }
}