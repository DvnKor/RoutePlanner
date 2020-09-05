using System;

namespace RoutePlannerApi.Domain
{
    public class Coordinate
    {
        public readonly int Latitude;
        public readonly int Longitude;
        private const int MaxX = 100;
        private const int MaxY = 100;
        private static readonly Random Random = new Random(5);
        public Coordinate(int latitude, int longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinate()
        {
            Latitude = Random.Next(0, MaxX);
            Longitude = Random.Next(0, MaxY);
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