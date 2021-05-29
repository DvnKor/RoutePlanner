using System;
using FluentAssertions;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Domain.RouteStepCalculator;
using Infrastructure.Common;
using NUnit.Framework;
using Storages.Extensions;

namespace GeneticAlgorithm.Tests
{
    [TestFixture]
    public class GoogleRouteStepCalculatorTests
    {
        private readonly IRouteStepCalculator _routeStepCalculator = new GoogleRouteStepCalculator();

        [Test]
        public void CalculateRouteStep()
        {
            var from = new Coordinate
            {
                Latitude = 56.794021,
                Longitude = 60.472204
            };
            var to = new Coordinate
            {
                Latitude = 56.823818,
                Longitude = 60.663204
            };
            var departureTime = DateTime.Now.StartOfNextWeek().Date.AddDays(1).AddHours(7);

            var routeStep = _routeStepCalculator.CalculateRouteStep(
                from, to, departureTime);

            Console.WriteLine(routeStep);
            routeStep.distance.Should().BeGreaterOrEqualTo(17900).And.BeLessThan(18500);
            routeStep.time.Should().BeGreaterOrEqualTo(50).And.BeLessThan(65);
        }
    }
}