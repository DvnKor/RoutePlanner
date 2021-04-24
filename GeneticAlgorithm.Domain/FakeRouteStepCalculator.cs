using System;
using GeneticAlgorithm.Contracts;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain
{
    public class FakeRouteStepCalculator : IRouteStepCalculator
    {
        private const double EarthRadius = 6376500.0;

        /// <summary>
        /// Средняя скорость менеджера в м/c
        /// </summary>
        private const double Velocity = 7;

        /// <summary>
        /// Временной запас в минутах
        /// </summary>
        private const double ExtraMinutes = 10; 
        
        public (double distance, double time) CalculateRouteStep(Coordinate from, Coordinate to)
        {
            var distance = GetDistance(from, to);

            var time = distance / Velocity + ExtraMinutes;

            return (distance, time);
        }

        /// <summary>
        /// Возвращает расстояние между двумя координатами
        /// </summary>
        private static double GetDistance(Coordinate from, Coordinate to)
        {
            var d1 = from.Latitude * (Math.PI / 180.0);
            var num1 = from.Longitude * (Math.PI / 180.0);
            var d2 = to.Latitude * (Math.PI / 180.0);
            var num2 = to.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) 
                     + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return EarthRadius * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}