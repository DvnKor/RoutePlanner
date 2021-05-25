using Google.Maps;
using Google.Maps.Common;
using Google.Maps.DistanceMatrix;
using Newtonsoft.Json;

namespace GeneticAlgorithm.Domain.RouteStepCalculator
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DistanceMatrixAdvancedResponse : IServiceResponse
    {
        [JsonProperty("status")]
        public ServiceResponseStatus Status { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("destination_addresses")]
        public string[] DestinationAddresses { get; set; }

        [JsonProperty("origin_addresses")]
        public string[] OriginAddresses { get; set; }

        [JsonProperty("rows")]
        public DistanceMatrixAdvancedRows[] Rows { get; set; }

        [JsonObject(MemberSerialization.OptIn)]
        public class DistanceMatrixAdvancedRows
        {
            [JsonProperty("elements")]
            public DistanceMatrixAdvancedElement[] Elements { get; set; }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class DistanceMatrixAdvancedElement
        {
            [JsonProperty("status")]
            public ServiceResponseStatus Status { get; set; }

            [JsonProperty("distance")]
            public ValueText Distance { get; set; }

            [JsonProperty("duration")]
            public ValueText Duration { get; set; }
            
            [JsonProperty("duration_in_traffic")]
            public ValueText DurationInTraffic { get; set; }
        }
    }
}