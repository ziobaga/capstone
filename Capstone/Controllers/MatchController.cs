using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.Enums;
using Capstone.Models.ViewModels;
using Capstone.Services.Match;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Capstone.Controllers
{

    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly DataContext _context;

        public MatchController(IMatchService matchService, DataContext dbContext)
        {
            _context = dbContext;
        }

        // 1. GET: Visualizzare il form per creare una nuova partita

        // GET: Match/Create
        public IActionResult CreateMatch()
        {
            // Recupera i campi dal database per popolare il dropdown
            var campi = _context.Fields.ToList();

            // Passa i campi alla vista utilizzando ViewBag
            ViewBag.Campi = campi;

            // Ritorna la vista con il modello Matches
            return View(new Matches());
        }

        // 2. POST: Creare una nuova partita

        // POST: Match/Create
        [HttpPost]
        public async Task<IActionResult> CreateMatch([FromForm] Matches match)
        {
            try
            {
                // Ottieni l'ID dell'utente autenticato (da token o sessione)
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Assegna l'ID dell'utente come CreatoreId
                match.CreatoreId = userId;

                // Combina la data con l'ora di inizio e l'ora di fine
                match.DataInizio = match.DataInizio.Date.Add(match.OraInizio);
                var dataFine = match.DataInizio.Date.Add(match.OraFine);

                // Imposta lo stato e la chat
                match.Stato = StatoPartita.InProgramma;
                match.Chat = new Chats { DataCreazione = DateTime.Now };

                // Verifica che l'ora di fine sia successiva all'ora di inizio
                if (dataFine <= match.DataInizio)
                {
                    return BadRequest("L'ora di fine deve essere successiva all'ora di inizio.");
                }

                _context.Matches.Add(match);
                await _context.SaveChangesAsync();

                return RedirectToAction("MatchList");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 3. GET: Visualizzare la lista di partite esistenti
        [HttpGet]
        public async Task<IActionResult> MatchList()
        {
            try
            {
                // Ottieni l'ID dell'utente autenticato
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Recupera le partite create dall'utente loggato
                var userMatches = await _context.Matches
                    .Include(m => m.Campo)
                    .Include(m => m.Partecipanti)
                    .Where(m => m.CreatoreId == userId) // Filtra per l'ID dell'utente
                    .ToListAsync();

                // Recupera le partite create da altri utenti
                var otherMatches = await _context.Matches
                    .Include(m => m.Campo)
                    .Include(m => m.Partecipanti)
                    .Where(m => m.CreatoreId != userId) // Filtra per gli altri utenti
                    .ToListAsync();

                // Crea un oggetto ViewModel per passare entrambe le liste alla vista
                var viewModel = new MatchListViewModel
                {
                    UserMatches = userMatches,
                    OtherMatches = otherMatches
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. GET: Recuperare i dettagli di una partita specifica
        [HttpGet("details/{id}")]
        public async Task<IActionResult> MatchDetails(int id)
        {
            try
            {
                var match = await _context.Matches
                    .Include(m => m.Campo)
                    .Include(m => m.Creatore)
                    .Include(m => m.Partecipanti)
                    .Include(m => m.Chat)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (match == null)
                    return NotFound("Partita non trovata");

                return View(match); // Torna alla view, non Ok per una vista dettagli
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Match/GetMatchById/5
        [HttpGet("getmatchbyid/{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            try
            {
                var match = await _context.Matches
                    .Include(m => m.Campo)
                    .Include(m => m.Creatore)
                    .Include(m => m.Partecipanti)
                    .Include(m => m.Chat)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (match == null)
                    return NotFound("Partita non trovata");

                return Ok(match); // Questa azione restituisce JSON
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Match/Edit/5
        [HttpGet]
        public async Task<IActionResult> EditMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Campo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            // Ottieni l'ID dell'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verifica se l'utente è il creatore della partita
            if (match.CreatoreId != userId)
            {
                return Forbid("Non hai il permesso di modificare questa partita.");
            }

            // Recupera i campi disponibili per popolare il dropdown
            var campi = _context.Fields.ToList();
            ViewBag.Campi = campi;

            return View(match);
        }

        // 5. POST: Modificare una partita creata dall'utente (solo dal creatore)
        // POST: Match/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditMatch(int id, Matches model)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            // Ottieni l'ID dell'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verifica se l'utente è il creatore della partita
            if (match.CreatoreId != userId)
            {
                return Forbid("Non hai il permesso di modificare questa partita.");
            }

            // Aggiorna i dettagli della partita
            match.DataInizio = model.DataInizio;
            match.OraInizio = model.OraInizio;
            match.OraFine = model.OraFine;
            match.TipoPartita = model.TipoPartita;
            match.CampoId = model.CampoId;

            await _context.SaveChangesAsync();
            return RedirectToAction("MatchList");
        }


        // 6. DELETE: Annullare una partita (solo dal creatore)
        // GET: Match/Delete/5
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Campo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            // Ottieni l'ID dell'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verifica se l'utente è il creatore della partita
            if (match.CreatoreId != userId)
            {
                return Forbid("Non hai il permesso di eliminare questa partita.");
            }

            return View(match); // Mostra la view di conferma eliminazione
        }

        // POST: Match/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            // Ottieni l'ID dell'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verifica se l'utente è il creatore della partita
            if (match.CreatoreId != userId)
            {
                return Forbid("Non hai il permesso di eliminare questa partita.");
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            // Reindirizza alla lista delle partite dopo la cancellazione
            return RedirectToAction(nameof(MatchList));
        }

    }
}
