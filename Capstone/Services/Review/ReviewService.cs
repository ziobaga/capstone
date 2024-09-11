using Capstone.Models;
using Capstone.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _context;

        public ReviewService(DataContext context)
        {
            _context = context;
        }

        // Aggiungi una nuova recensione e aggiorna la media delle valutazioni del campo
        public async Task<Reviews> AddReviewAsync(Reviews review)
        {
            // Aggiungi la nuova recensione
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            // Se la recensione è associata a un campo, aggiorna la valutazione media
            if (review.ValutatoCampoId.HasValue)
            {
                var field = await _context.Fields
                    .Include(f => f.Reviews) // Carica le recensioni associate
                    .FirstOrDefaultAsync(f => f.Id == review.ValutatoCampoId);

                if (field != null)
                {
                    // Ricalcola la media delle valutazioni e cast esplicito da double a decimal
                    field.ValutazioneMedia = (decimal)field.Reviews.Average(r => r.Punteggio);
                    await _context.SaveChangesAsync();
                }
            }

            return review;
        }

        public Task<IEnumerable<Reviews>> GetReviewsByFieldIdAsync(int fieldId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reviews>> GetReviewsByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
