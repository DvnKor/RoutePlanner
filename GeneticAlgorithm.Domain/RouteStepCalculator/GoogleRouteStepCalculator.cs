using System;
using System.Configuration;
using System.Linq;
using GeneticAlgorithm.Contracts;
using Google.Maps;
using Infrastructure.Common;

namespace GeneticAlgorithm.Domain.RouteStepCalculator
{
    public class GoogleRouteStepCalculator : IRouteStepCalculator
    {
        private const string RussianLanguage = "ru";
        private readonly DistanceMatrixAdvancedService _distanceMatrixAdvancedService;

        public GoogleRouteStepCalculator()
        {
            var mapsApiKey = 
                Environment.GetEnvironmentVariable("GOOGLE_MAPS_API_KEY") ??
                ConfigurationManager.AppSettings.Get("GoogleMapsApiKey");

            GoogleSigned.AssignAllServices(new GoogleSigned(mapsApiKey));
            _distanceMatrixAdvancedService = new DistanceMatrixAdvancedService();
        }

        public (double distance, double time) CalculateRouteStep(
            Coordinate from, Coordinate to, DateTime departureTime)
        {
            var matrixRequest = new DistanceMatrixAdvancedRequest
            {
                Language = RussianLanguage,
                Mode = TravelMode.driving,
                DepartureTime = departureTime
            };
            matrixRequest.AddOrigin(new LatLng(from.Latitude, from.Longitude));
            matrixRequest.AddDestination(new LatLng(to.Latitude, to.Longitude));
            var response = _distanceMatrixAdvancedService.GetResponse(matrixRequest);
            if (response.Status == ServiceResponseStatus.Ok)
            {
                var element = response.Rows.FirstOrDefault()?.Elements?.FirstOrDefault();
                if (element != null && element.Status == ServiceResponseStatus.Ok)
                {
                    // toDO надо как-то получить duration_in_traffic
                    var distanceInMeters = element.Distance.Value;
                    var timeInSeconds = element.DurationInTraffic.Value;
                    var timeInMinutes = timeInSeconds / 60 + 1;
                    return (distanceInMeters, timeInMinutes);
                }

                throw new InvalidOperationException($"Element is null or have bad status {element?.Status}");
            }

            Console.WriteLine("Произошла ошибка при запросе к Google DistanceMatrix API");
            Console.WriteLine($"Статус ответа {response.Status}");
            Console.WriteLine($"Сообщение об ошибке: {response.ErrorMessage}");
            throw new InvalidOperationException(response.ErrorMessage);
        }
    }
}