using System;
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
        private readonly Random _random = new ();
        private readonly DateTime _dateTime = DateTime.Now.AddMonths(-1).Date;
        private readonly IRoutePlanner _routePlanner = TestRoutePlannerFactory.Create();
        
        [Test]
        public void GetBestRoute()
        {
            var managerSchedules = Enumerable.Range(0, 5)
                .Select(GetRandomManagerSchedule)
                .ToList();
            var meetings = Enumerable.Range(0, 25)
                .Select(GetRandomMeeting)
                .ToList();

            var progress = _routePlanner.GetBestRoutesProgress(
                managerSchedules,
                meetings)
                .ToList();

            var i = 0;
            Console.WriteLine("Прогресс");
            foreach (var genotype in progress)
            {
                Console.WriteLine($"{i}. Фитнес: {genotype.Fitness}. Кол-во встреч: {genotype.SuitableMeetingsCount}. Расстояние: {genotype.Distance} м. Время ожидания: {genotype.WaitingTime} минут");
                i++;
            }
            Console.WriteLine();

            var bestRoutes = progress.OrderByDescending(x => x.SuitableMeetingsCount).First();
            Console.WriteLine(bestRoutes.PrintRoutesWithParameters());
        }

        private Meeting GetRandomMeeting(int clientId)
        {
            var availableTimeStart = _dateTime.AddHours(_random.Next(7, 19)).AddMinutes(_random.Next(0, 60));
            var availableTimeEnd = availableTimeStart.AddMinutes(_random.Next(60, 300));
            var durationInMinutes = _random.Next(15, 120);
            return new Meeting
            {
                ClientId = clientId,
                AvailableTimeStart = availableTimeStart,
                AvailableTimeEnd = availableTimeEnd,
                DurationInMinutes = durationInMinutes,
                Coordinate = GetRandomCoordinate()
            };
        }

        private ManagerSchedule GetRandomManagerSchedule(int userId)
        {
            return new()
            {
                UserId = userId,
                StartCoordinate = GetRandomCoordinate(),
                EndCoordinate = GetRandomCoordinate(),
                StartTime = _dateTime.AddHours(_random.Next(7, 10)),
                EndTime = _dateTime.AddHours(_random.Next(17, 20)),
            };
        }
        
        private Coordinate GetRandomCoordinate()
        {
            return new()
            {
                Latitude = _random.NextDouble() / 50 + 56.8519,
                Longitude = _random.NextDouble() / 50 + 60.6122
            };
        }
    }
}