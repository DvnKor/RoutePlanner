using System;
using System.Threading;
using FluentAssertions;
using Infrastructure.Common;
using NUnit.Framework;

namespace Infrastructure.Cache.Tests
{
    [TestFixture]
    public class ExpiringCacheTests
    {
        [Test]
        public void CacheTest()
        {
            var count = 0;
            
            int CacheValueFactory(
                (Coordinate from, Coordinate to, DateTime departureTime) parameters)
            {
                return ++count;
            }
            
            var routeStepCache = CacheFactory
                .CreateExpiringCache<(Coordinate, Coordinate, DateTime), int>(
                    CacheValueFactory, TimeSpan.FromSeconds(2));
            
            var from = new Coordinate
            {
                Latitude = 56.8519,
                Longitude = 60.6122
            };
            var to = new Coordinate
            {
                Latitude = 56.8619,
                Longitude = 60.6120
            };
            var time = DateTime.UtcNow;

            
            var firstCallResult = routeStepCache.Get(
                (from, to, time));
            var secondCallResult = routeStepCache.Get(
                (from, to, time));
            firstCallResult.Should().Be(1);
            secondCallResult.Should().Be(1);
            Thread.Sleep(2000);
            
            var expiredCallResult = routeStepCache.Get(
                (from, to, time));
            expiredCallResult.Should().Be(2);
        }
    }
}