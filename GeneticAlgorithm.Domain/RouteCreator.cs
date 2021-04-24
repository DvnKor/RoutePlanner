using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using Infrastructure.Cache;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain
{
    public class RouteCreator : IRouteCreator
    {
        private readonly IFitnessCalculator _fitnessCalculator;
        private readonly IRouteStepCalculator _routeStepCalculator;
        private readonly ExpiringCache<(Coordinate, Coordinate), (double, double)> _routeStepCache;

        public RouteCreator(IFitnessCalculator fitnessCalculator, IRouteStepCalculator routeStepCalculator)
        {
            _fitnessCalculator = fitnessCalculator;
            _routeStepCalculator = routeStepCalculator;
            // Расчет расстояния и времени в пути между двумя координатами кэшируется на 5 минут
            _routeStepCache = CacheFactory
                .CreateExpiringCache<(Coordinate, Coordinate), (double, double)>(RouteStepCacheValueFactory, 5);
        }

        public Route Create(ManagerSchedule managerSchedule, List<Meeting> possibleMeetings)
        {
            var managerEndFakeMeeting = new Meeting
            {
                StartTime = managerSchedule.EndTime,
                EndTime = managerSchedule.EndTime,
                Coordinate = managerSchedule.EndCoordinate
            };

            var allMeetings = possibleMeetings.Concat(new[] {managerEndFakeMeeting});
            
            var suitableMeetings = new List<Meeting>();
            var currentCoordinate = managerSchedule.StartCoordinate;
            var currentTime = managerSchedule.StartTime;
            var pathDistance = 0d;
            var waitingTime = new TimeSpan();
            
            foreach (var nextMeeting in allMeetings)
            {
                // Менеджер не успвает провести встречу до конца рабочего дня
                if (nextMeeting.EndTime > managerSchedule.EndTime) continue;

                var nextCoordinate = nextMeeting.Coordinate;
                var (distanceToNext, timeToNext) = _routeStepCache.Get(
                    (currentCoordinate, nextCoordinate));

                var arrivalTime = currentTime.AddMinutes(timeToNext);

                // Менеджер не успевает на встречу
                if (arrivalTime > nextMeeting.StartTime) continue;
                
                // Перед встречей остаётся свободное время
                if (arrivalTime < nextMeeting.StartTime)
                {
                    waitingTime += nextMeeting.StartTime - arrivalTime;
                }
                
                suitableMeetings.Add(nextMeeting);
                pathDistance += distanceToNext;
                currentTime = nextMeeting.EndTime;
                currentCoordinate = nextCoordinate;
            }

            var waitingTimeMinutes = waitingTime.TotalMinutes;
            var routeFinishesAsPreferred = suitableMeetings.Last() == managerEndFakeMeeting;
            var fitness = _fitnessCalculator.Calculate(
                suitableMeetings.Count,
                pathDistance, 
                waitingTimeMinutes,
                routeFinishesAsPreferred);
            
            return new Route
            {
                ManagerScheduleId = managerSchedule.Id,
                ManagerSchedule = managerSchedule,
                PossibleMeetings = possibleMeetings,
                SuitableMeetings = suitableMeetings,
                Distance = pathDistance,
                WaitingTime = waitingTimeMinutes,
                Fitness = fitness
            };
        }
        
        private (double distance, double time) RouteStepCacheValueFactory((Coordinate from, Coordinate to) coordinates)
        {
            var (from, to) = coordinates;
            return _routeStepCalculator.CalculateRouteStep(from, to);
        }
    }
}