using System;
using System.Linq;
using System.Threading.Tasks;
using GeneticAlgorithm.Contracts;
using Infrastructure.Common;
using Storages;

namespace RoutePlannerDaemon
{
    public class RoutesUpdater
    {
        private readonly TimeSpan _reserveMeetingTime = TimeSpan.FromMinutes(40);

        private readonly IMeetingStorage _meetingStorage;
        private readonly IManagerScheduleStorage _managerScheduleStorage;
        private readonly IRouteStorage _routeStorage;
        private readonly IRoutePlanner _routePlanner;
        
        public RoutesUpdater(
            IMeetingStorage meetingStorage,
            IManagerScheduleStorage managerScheduleStorage,
            IRouteStorage routeStorage,
            IRoutePlanner routePlanner)
        {
            _meetingStorage = meetingStorage;
            _managerScheduleStorage = managerScheduleStorage;
            _routePlanner = routePlanner;
            _routeStorage = routeStorage;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                try
                {
                    await UpdateRoutes();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private async Task UpdateRoutes()
        {
            var now = DateTime.UtcNow.AddHours(TimezoneProvider.OffsetInHours);
            Console.WriteLine($"Началось обновление маршрутов {now:g}");

            var algorithmStartTime = now + _reserveMeetingTime;
            var meetings = await _meetingStorage.GetPossibleMeetings(algorithmStartTime);

            if (meetings.Length == 0)
            {
                return;
            }
            
            var managerSchedules = await _managerScheduleStorage.GetManagerSchedules(now);
            if (managerSchedules.Length == 0)
            {
                return;
            }
            
            var managerSchedulesIds = managerSchedules
                .Select(managerSchedule => managerSchedule.Id)
                .ToArray();

            var currentRoutes = await _routeStorage.GetRoutesByScheduleIds(managerSchedulesIds);

            foreach (var managerSchedule in managerSchedules)
            {
                var routeExist = currentRoutes.TryGetValue(managerSchedule.Id, out var route);
                if (routeExist)
                {
                    var currentMeeting = route.SuitableMeetings
                        .Where(meeting => meeting.StartTime <= algorithmStartTime)
                        .OrderByDescending(meeting => meeting.StartTime)
                        .FirstOrDefault();
                    if (currentMeeting != null)
                    {
                        managerSchedule.StartTime = currentMeeting.EndTime;
                        managerSchedule.StartCoordinate = currentMeeting.Coordinate;
                    }
                }
            }
            
            var bestRoutes = _routePlanner.GetBestRoutes(managerSchedules, meetings);

            Console.WriteLine(bestRoutes.PrintRoutesWithParameters());

            foreach (var route in bestRoutes.Routes)
            {
                await _routeStorage.AddOrUpdateRoute(route);
            }

            //toDo удалять старые маршруты 
        }
    }
}