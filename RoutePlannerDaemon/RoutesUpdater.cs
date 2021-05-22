using System;
using System.Threading.Tasks;
using GeneticAlgorithm.Contracts;
using Storages;

namespace RoutePlannerDaemon
{
    public class RoutesUpdater
    {
        private const int GenerationCount = 500;
        private const int PopulationSize = 100;
        private const int  EliteSize = 15;
        private const double MutationRate = 0.1;

        private readonly IMeetingStorage _meetingStorage;
        private readonly IManagerScheduleStorage _managerScheduleStorage;
        private readonly IRoutePlanner _routePlanner;
        
        public RoutesUpdater(
            IMeetingStorage meetingStorage,
            IManagerScheduleStorage managerScheduleStorage,
            IRoutePlanner routePlanner)
        {
            _meetingStorage = meetingStorage;
            _managerScheduleStorage = managerScheduleStorage;
            _routePlanner = routePlanner;
        }

        public async Task RunAsync()
        {
            var now = DateTime.UtcNow.AddHours(5);
            var meetings = await _meetingStorage.GetMeetings(now);
            var managerSchedules = await _managerScheduleStorage.GetManagerSchedules(now);
            
            var bestRoutes = _routePlanner.GetBestRoutes(
                managerSchedules,
                meetings,
                GenerationCount,
                PopulationSize,
                EliteSize,
                MutationRate);

            foreach (var route in bestRoutes.Routes)
            {
                // toDo save
                //route.ManagerScheduleId
            }
        }
    }
}