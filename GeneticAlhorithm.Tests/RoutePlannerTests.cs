using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using GeneticAlgorithm.Contracts;
using Infrastructure.Common;
using NUnit.Framework;

namespace GeneticAlgorithm.Tests
{
    [TestFixture]
    public class RoutePlannerTests
    {
        private readonly Random _random = new Random();
        private readonly DateTime _dateTime = DateTime.Now.Date;
        private readonly IRoutePlanner _routePlanner = TestRoutePlannerFactory.Create();
        
        [Test]
        public void GetBestRoute()
        {
            var managerSchedules = Enumerable.Range(0, 3)
                .Select(_ => GetRandomManagerSchedule())
                .ToList();
            var clients = Enumerable.Range(0, 30)
                .Select(_ => GetRandomMeeting())
                .ToList();
            var generationCount = 500;
            var populationSize = 100;
            var eliteSize = 50;
            var mutationRate = 0.1;

            var bestRoutes = _routePlanner.GetBestRoutes(
                managerSchedules,
                clients,
                generationCount,
                populationSize,
                eliteSize,
                mutationRate);
            
            Console.WriteLine($"Fitness: {bestRoutes.Fitness}");
            Console.WriteLine($"Distance: {bestRoutes.Distance}");
            Console.WriteLine($"WaitingTime: {bestRoutes.WaitingTime}");
            Console.WriteLine($"SuitableMeetingsCount: {bestRoutes.SuitableMeetingsCount}");
        }

        private Meeting GetRandomMeeting()
        {
            var startTime = _dateTime.AddHours(_random.Next(7, 20)).AddMinutes(_random.Next(0, 60));
            var endTime = startTime.AddMinutes(_random.Next(15, 120));
            return new Meeting
            {
                StartTime = startTime,
                EndTime = endTime,
                Coordinate = GetRandomCoordinate()
            };
        }

        private ManagerSchedule GetRandomManagerSchedule()
        {
            return new ManagerSchedule
            {
                StartCoordinate = GetRandomCoordinate(),
                EndCoordinate = GetRandomCoordinate(),
                StartTime = _dateTime.AddHours(_random.Next(7, 10)),
                EndTime = _dateTime.AddHours(_random.Next(17, 20))
            };
        }
        
        private Coordinate GetRandomCoordinate()
        {
            return new Coordinate
            {
                Latitude = _random.NextDouble() / 2 + 60,
                Longitude = _random.NextDouble() / 2 + 55
            };
        }
    }
}