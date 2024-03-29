using System;
using System.Threading.Tasks;
using Google.Maps;
using Google.Maps.Internal;

namespace GoogleMaps.Client
{
    public class DistanceMatrixAdvancedService : IDisposable
    {
        private static readonly Uri HttpsUri
            = new("https://maps.google.com/maps/api/distancematrix/");

        private readonly Uri _baseUri;
        private MapsHttp _http;

        public DistanceMatrixAdvancedService(GoogleSigned signingSvc = null, Uri baseUri = null)
        {
            _baseUri = baseUri ?? HttpsUri;
            _http = new MapsHttp(signingSvc ?? GoogleSigned.SigningInstance);
        }

        public DistanceMatrixAdvancedResponse GetResponse(DistanceMatrixAdvancedRequest request)
        {
            return _http.Get<DistanceMatrixAdvancedResponse>(
                new Uri(_baseUri, request.ToUri()));
        }

        public async Task<DistanceMatrixAdvancedResponse> GetResponseAsync(
            DistanceMatrixAdvancedRequest request)
        {
            return await _http.GetAsync<DistanceMatrixAdvancedResponse>(
                new Uri(_baseUri, request.ToUri()));
        }

        public void Dispose()
        {
            if (_http == null) return;
            _http.Dispose();
            _http = null;
        }
    }
}