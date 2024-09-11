using Newtonsoft.Json.Linq;

namespace Capstone.Services.GoogleMapsAPI
{
    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoogleMapsService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> GetCoordinatesFromAddressAsync(string address)
        {
            string apiKey = _configuration["GoogleMaps:ApiKey"];
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={apiKey}";

            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);

            var location = json["results"]?[0]?["geometry"]?["location"];
            if (location != null)
            {
                var lat = location["lat"].ToString();
                var lng = location["lng"].ToString();
                return $"{lat},{lng}";
            }

            return null;
        }
    }
}
