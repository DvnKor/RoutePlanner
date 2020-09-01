using System;

namespace RoutePlannerApi.Domain
{
    public class GeoCoordinate : ICoordinate
    {
        public double Longitude;
        public double Latitude;
        public TimeSpan GetTravelTime(ICoordinate otherCoordinate)
        {
            throw new NotImplementedException();
        }
    }
}