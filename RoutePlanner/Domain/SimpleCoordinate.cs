using System;

namespace RoutePlanner
{
    public class SimpleCoordinate : ICoordinate
    {
        public readonly int X;
        public readonly int Y;
        private const int maxX = 100;
        private const int maxY = 100;
        private static readonly Random random = new Random(5);
        public SimpleCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public SimpleCoordinate()
        {
            X = random.Next(0, maxX);
            Y = random.Next(0, maxY);
        }
        public TimeSpan GetTravelTime(ICoordinate otherCoordinate)
        {
            if (!(otherCoordinate is SimpleCoordinate))
            {
                throw new ArgumentException("Cannot count travel time from another coordinate type");
            }
            var otherSimpleCoordinate = (SimpleCoordinate) otherCoordinate;
            return TimeSpan.FromMinutes(Math.Abs(X - otherSimpleCoordinate.X) + Math.Abs(Y - otherSimpleCoordinate.Y));
        }

        public override string ToString()
        {
           return $"{X},{Y}";
        }
    }
}