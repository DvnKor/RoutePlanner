using System;
using Google.Maps.DistanceMatrix;

namespace GoogleMaps.Client
{
    public class DistanceMatrixAdvancedRequest : DistanceMatrixRequest
    {
        public DateTime DepartureTime { get; set; }
        
        public override Uri ToUri()
        {
            var uriString = base.ToUri().ToString();
            if (DepartureTime != default)
            {
                var departureTimeInSeconds = new DateTimeOffset(DepartureTime)
                    .ToUnixTimeSeconds();
                var nowTimeInSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var departureTimeQuery =
                    departureTimeInSeconds < nowTimeInSeconds ? "now" : $"{departureTimeInSeconds}";
                uriString += $"&departure_time={departureTimeQuery}";   
            }

            return new Uri(uriString, UriKind.Relative);
        }
    }
}