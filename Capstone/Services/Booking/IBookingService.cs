using Capstone.Models;

namespace Capstone.Services.Booking
{
    public interface IBookingService
    {
        Task<Bookings> GetBookingByIdAsync(int id);
        Task<IEnumerable<Bookings>> GetAllBookingsAsync();
        Task<Bookings> CreateBookingAsync(Bookings booking);
        Task<bool> UpdateBookingAsync(Bookings booking);
        Task<bool> DeleteBookingAsync(int id);
        Task<bool> ProcessPaymentAsync(decimal amount, string currency, string paymentMethodId);
    }
}
