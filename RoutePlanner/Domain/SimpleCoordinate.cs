using System;

namespace RoutePlanner
{
    public class SimpleCoordinate : ICoordinate
    {
        public readonly int X;
        public readonly int Y;
        public SimpleCoordinate(int x, int y)
        {
            X = x;
            Y = y;
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
    }
}