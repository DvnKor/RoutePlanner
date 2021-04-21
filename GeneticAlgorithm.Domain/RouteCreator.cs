using System;
using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Contracts;

namespace GeneticAlgorithm.Domain
{
    public class RouteCreator : IRouteCreator
    {
        private readonly IFitnessCalculator _fitnessCalculator;
        private readonly IRouteStepCalculator _routeStepCalculator;

        public RouteCreator(IFitnessCalculator fitnessCalculator, IRouteStepCalculator routeStepCalculator)
        {
            _fitnessCalculator = fitnessCalculator;
            _routeStepCalculator = routeStepCalculator;
        }

        public Route Create(ManagerSchedule managerSchedule, List<Meeting> possibleMeetings)
        {
            //toDo добавить фейковую встречу в конце пути менеджера
            
            var suitableMeetings = new List<Meeting>();
            var currentTime = managerSchedule.StartTime;
            var pathDistance = 0d;
            var waitingTime = new TimeSpan();
            
            //toDo обеспечить возвращение менеджера в нужную точку в конце маршрута
            foreach (var nextMeeting in possibleMeetings)
            {
                //toDO остальные проверки
                if (nextMeeting.EndTime > managerSchedule.EndTime) continue;
                
                var (distanceToNext, timeToNext) = _routeStepCalculator.CalculateRouteStep();

                var arrivalTime = currentTime.AddMinutes(timeToNext);

                if (arrivalTime > nextMeeting.StartTime) continue;
                
                if (arrivalTime < nextMeeting.StartTime)
                {
                    waitingTime += nextMeeting.StartTime - arrivalTime;
                }
                
                suitableMeetings.Add(nextMeeting);
                pathDistance += distanceToNext;
                currentTime = nextMeeting.EndTime;
            }

            var waitingTimeMinutes = waitingTime.TotalMinutes;
            var fitness = _fitnessCalculator.Calculate(
                suitableMeetings.Count,
                pathDistance, 
                waitingTimeMinutes);
            
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
    }
}