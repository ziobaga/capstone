namespace Capstone.Services.GoogleMapsAPI
{
    public interface IGoogleMapsService
    {
        Task<string> GetCoordinatesFromAddressAsync(string address);
    }
}
