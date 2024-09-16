using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.Enums;
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

        public async Task<Matches> CreateMatchAsync(Matches match)
        {
            match.Stato = StatoPartita.InProgramma;
            match.Chat = new Chats { DataCreazione = DateTime.Now }; // Crea la chat per la partita

            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();

            return match;
        }

        public async Task<Matches> GetMatchByIdAsync(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Campo)            // Includiamo il campo associato
                .Include(m => m.Partecipanti)     // Includiamo i partecipanti
                .Include(m => m.Chat)             // Includiamo la chat associata
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                throw new Exception("Partita non trovata");
            }

            return match;
        }

        public async Task<List<Matches>> GetMatchesAsync()
        {
            return await _context.Matches
                .Include(m => m.Campo)
                .Include(m => m.Partecipanti)
                .Include(m => m.Chat)
                .ToListAsync();
        }

        public async Task<Matches> UpdateMatchAsync(int matchId, Matches updatedMatch)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId && m.CreatoreId == updatedMatch.CreatoreId);
            if (match == null)
            {
                throw new Exception("Partita non trovata o permesso negato");
            }

            match.DataInizio = updatedMatch.DataInizio;
            match.OraInizio = updatedMatch.OraInizio;
            match.OraFine = updatedMatch.OraFine;
            match.TipoPartita = updatedMatch.TipoPartita;
            match.CampoId = updatedMatch.CampoId;

            await _context.SaveChangesAsync();
            return match;
        }

        public async Task CancelMatchAsync(int matchId, int userId)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId && m.CreatoreId == userId);
            if (match == null)
            {
                throw new Exception("Partita non trovata o permesso negato");
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }
    }
}
