using Capstone.Models;

namespace Capstone.Services.Match
{
    public interface IMatchService
    {
        Task<Matches> GetMatchByIdAsync(int id);
        Task<List<Matches>> GetMatchesAsync();
        Task<Matches> CreateMatchAsync(Matches match);
        Task<Matches> UpdateMatchAsync(int matchId, Matches updatedMatch);
        Task DeleteMatchAsync(int matchId, int userId);
    }
}

