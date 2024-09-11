using Capstone.Models;
using Capstone.Models.Context;
using Stripe;

namespace Capstone.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public BookingService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Bookings> CreateBookingAsync(Bookings booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public Task<bool> DeleteBookingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Bookings>> GetAllBookingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Bookings> GetBookingByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ProcessPaymentAsync(decimal amount, string currency, string paymentMethodId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),  // Convert to cents
                Currency = currency,
                PaymentMethod = paymentMethodId,
                Confirm = true
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent.Status == "succeeded";
        }

        public Task<bool> UpdateBookingAsync(Bookings booking)
        {
            throw new NotImplementedException();
        }
    }
}
