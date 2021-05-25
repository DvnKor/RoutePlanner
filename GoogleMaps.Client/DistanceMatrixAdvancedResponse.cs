using Google.Maps;
using Google.Maps.DistanceMatrix;
using Newtonsoft.Json;

namespace GoogleMaps.Client
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DistanceMatrixAdvancedResponse : DistanceMatrixResponse
    {
        [JsonProperty("rows")]
        public new DistanceMatrixAdvancedRows[] Rows { get; set; }

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