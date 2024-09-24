using Microsoft.AspNetCore.Mvc;

namespace Capstone.Services.GoogleMapsAPI
{
    public interface IGoogleMapsService
    {
        Task<IActionResult> CampiVicinanze(double lat, double lng);

        Task GetDistance(double lat1, double lon1, double lat2, double lon2);

        Task Deg2Rad(double deg);
    }
}
