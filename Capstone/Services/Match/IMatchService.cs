using Capstone.Models;

namespace Capstone.Services.Match
{
    public interface IMatchService
    {
        Task<Matches> GetMatchByIdAsync(int id);
        Task<IEnumerable<Matches>> GetAllMatchesAsync();
        Task<Matches> CreateMatchAsync(Matches match);
        Task<bool> UpdateMatchAsync(Matches match);
        Task<bool> DeleteMatchAsync(int id);
    }
}
