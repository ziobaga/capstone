using Capstone.Models;
using Capstone.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Services.Match
{
    public class MatchService : IMatchService
    {
        private readonly DataContext _context;

        public MatchService(DataContext context)
        {
            _context = context;
        }

        public async Task<Matches> GetMatchByIdAsync(int id)
        {
            return await _context.Matches
                                 .Include(m => m.Campo) // Include per caricare entità correlate
                                 .Include(m => m.Partecipanti)
                                 .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Matches>> GetAllMatchesAsync()
        {
            return await _context.Matches
                                 .Include(m => m.Campo)
                                 .Include(m => m.Partecipanti)
                                 .ToListAsync();
        }

        public async Task<Matches> CreateMatchAsync(Matches match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task<bool> UpdateMatchAsync(Matches match)
        {
            var existingMatch = await _context.Matches.FindAsync(match.Id);

            if (existingMatch == null)
            {
                return false;
            }

            existingMatch.DataInizio = match.DataInizio;
            existingMatch.DataFine = match.DataFine;
            existingMatch.TipoPartita = match.TipoPartita;
            existingMatch.Stato = match.Stato;
            existingMatch.CampoId = match.CampoId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMatchAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id);

            if (match == null)
            {
                return false;
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
