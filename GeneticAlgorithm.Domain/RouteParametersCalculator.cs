using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using Infrastructure.Cache;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain
{
    public class RouteParametersCalculator : IRouteParametersCalculator
    {
        private readonly IRouteStepCalculator _routeStepCalculator;
        private readonly ExpiringCache<(Coordinate, Coordinate, DateTime), (double, double)> _routeStepCache;

        public RouteParametersCalculator(IRouteStepCalculator routeStepCalculator)
        {
            _routeStepCalculator = routeStepCalculator;
            // Расчет расстояния и времени в пути между двумя координатами кэшируется на 10 минут
            _routeStepCache = CacheFactory
                .CreateExpiringCache<(Coordinate, Coordinate, DateTime), (double, double)>(
                    RouteStepCacheValueFactory, TimeSpan.FromMinutes(10));
        }

        public void CalculateParameters(Route route, List<Meeting> takenMeetings)
        {
            var managerSchedule = route.ManagerSchedule;
            var possibleMeetings = route.PossibleMeetings;
            var managerEndFakeMeeting = new Meeting
            {
                AvailableTimeStart = managerSchedule.EndTime,
                AvailableTimeEnd = managerSchedule.EndTime,
                DurationInMinutes = 0,
                Coordinate = managerSchedule.EndCoordinate
            };

            var allMeetings = possibleMeetings.Concat(new[] {managerEndFakeMeeting});
            
            var suitableMeetings = new List<Meeting>();
            var currentCoordinate = managerSchedule.StartCoordinate;
            var currentTime = managerSchedule.StartTime;
            var pathDistance = 0d;
            var waitingTime = TimeSpan.Zero;
            var routeFinishesAsPreferred = false;

            foreach (var nextMeeting in allMeetings)
            {
                // Встреча уже взята другим менеджером
                if (takenMeetings.Contains(nextMeeting)) continue;

                var nextMeetingDurationInMinutes = TimeSpan.FromMinutes(nextMeeting.DurationInMinutes);

                // Менеджер не успвает провести встречу до конца рабочего дня
                if (nextMeeting.AvailableTimeStart + nextMeetingDurationInMinutes > managerSchedule.EndTime) continue;

                var nextCoordinate = nextMeeting.Coordinate;
                var (distanceToNext, timeToNext) = _routeStepCache.Get(
                    (currentCoordinate, nextCoordinate, currentTime));

                var arrivalTime = currentTime.AddMinutes(timeToNext);
                var startTime = arrivalTime;
                // Перед встречей остаётся свободное время
                var currentWaitingTime = TimeSpan.Zero;
                if (arrivalTime < nextMeeting.AvailableTimeStart)
                {
                    currentWaitingTime = nextMeeting.AvailableTimeStart - arrivalTime;
                    waitingTime += currentWaitingTime;
                    startTime = nextMeeting.AvailableTimeStart;
                }

                var endTime = startTime + nextMeetingDurationInMinutes;
                // Менеджер не успевает провести встречу или не успевает ее провести до конца смены
                if (startTime > nextMeeting.AvailableTimeEnd - nextMeetingDurationInMinutes
                    || endTime > managerSchedule.EndTime)
                {
                    continue;
                }

                if (nextMeeting == managerEndFakeMeeting)
                {
                    routeFinishesAsPreferred = true;
                    break;
                }

                nextMeeting.StartTime = startTime;
                nextMeeting.EndTime = endTime;
                nextMeeting.DistanceFromPrevious = distanceToNext;
                nextMeeting.WaitingTime = currentWaitingTime.TotalMinutes;
                suitableMeetings.Add(nextMeeting);
                pathDistance += distanceToNext;
                currentTime = endTime;
                currentCoordinate = nextCoordinate;
            }

            var waitingTimeMinutes = waitingTime.TotalMinutes;

            route.SuitableMeetings = suitableMeetings;
            route.Distance = pathDistance;
            route.WaitingTime = waitingTimeMinutes;
            route.FinishesAsPreferred = routeFinishesAsPreferred;
        }
        
        private (double distance, double time) RouteStepCacheValueFactory(
            (Coordinate from, Coordinate to, DateTime departureTime) parameters)
        {
            var (from, to, departureTime) = parameters;
            return _routeStepCalculator.CalculateRouteStep(from, to, departureTime);
        }
    }
}