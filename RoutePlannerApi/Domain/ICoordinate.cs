using System;

namespace RoutePlannerApi.Domain
{
    public interface ICoordinate
    {
        TimeSpan GetTravelTime(ICoordinate otherCoordinate);

    }
}