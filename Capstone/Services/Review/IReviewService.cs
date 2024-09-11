using Capstone.Models;

namespace Capstone.Services.Review
{
    public interface IReviewService
    {
        // Metodo per aggiungere una nuova recensione e aggiornare la valutazione del campo
        Task<Reviews> AddReviewAsync(Reviews review);

        // Altri metodi, se necessario (es. ottenere recensioni)
        Task<IEnumerable<Reviews>> GetReviewsByFieldIdAsync(int fieldId);
        Task<IEnumerable<Reviews>> GetReviewsByUserIdAsync(int userId);
    }
}
