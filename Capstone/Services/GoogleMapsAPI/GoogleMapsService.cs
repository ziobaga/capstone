using Microsoft.AspNetCore.Mvc;

namespace Capstone.Services.GoogleMapsAPI
{
    public class GoogleMapsService : IGoogleMapsService
    {
        public Task<IActionResult> CampiVicinanze(double lat, double lng)
        {
            throw new NotImplementedException();
        }

        public Task Deg2Rad(double deg)
        {
            throw new NotImplementedException();
        }

        public Task GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            throw new NotImplementedException();
        }
    }
}
