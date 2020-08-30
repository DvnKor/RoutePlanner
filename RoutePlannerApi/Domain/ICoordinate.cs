using System;

namespace RoutePlanner
{
    public interface ICoordinate
    {
        TimeSpan GetTravelTime(ICoordinate otherCoordinate);

    }
}