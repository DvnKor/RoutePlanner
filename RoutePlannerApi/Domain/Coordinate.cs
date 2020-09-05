using System;

namespace RoutePlannerApi.Domain
{
    public class Coordinate
    {
        public readonly int Latitude;
        public readonly int Longitude;
        private const int maxX = 100;
        private const int maxY = 100;
        private static readonly Random random = new Random(5);
        public Coordinate(int latitude, int longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinate()
        {
            Latitude = random.Next(0, maxX);
            Longitude = random.Next(0, maxY);
        }
        public TimeSpan GetTravelTime(Coordinate otherCoordinate)
        {
            return TimeSpan.FromMinutes((Math.Abs(Latitude - otherCoordinate.Latitude) + Math.Abs(Longitude - otherCoordinate.Longitude)) * 2);
        }

        public override string ToString()
        {
           return $"{Latitude},{Longitude}";
        }
    }
}