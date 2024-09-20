using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.Enums;
using Capstone.Models.ViewModels;
using Capstone.Services.Match;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;

namespace Capstone.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly DataContext _context;

        public MatchController(IMatchService matchService, DataContext dbContext)
        {
            _matchService = matchService;
            _context = dbContext;
            StripeConfiguration.ApiKey = "sk_test_51Q0mODP563WlEe7GsBqEQglmxgCiUBn1hXytJSGH76JropG9s1n8f3eXxsHlLZ8lrN9DG3L4BFvMhbhMWTxMyaTJ00vmj2MlP4";
        }

        // 1. GET: Visualizzare il form per creare una nuova partita
        public IActionResult CreateMatch()
        {
            // Recupera i campi dal database per popolare il dropdown
            var campi = _context.Fields.ToList();
            ViewBag.Campi = campi;

            return View(new Matches());
        }

        // 2. POST: Creare una nuova partita
        [HttpPost]
        public async Task<IActionResult> CreateMatch([FromForm] Matches match)
        {
            try
            {
                // Ottieni l'ID dell'utente autenticato
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Imposta l'utente come creatore
                match.CreatoreId = userId;

                // Recupera l'utente creatore
                var creatore = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (creatore == null)
                {
                    return NotFound("Utente creatore non trovato.");
                }

                // Aggiungi l'utente creatore ai partecipanti
                //match.Partecipanti.Add(creatore);

                // Combina la data con l'ora di inizio e fine
                match.DataInizio = match.DataInizio.Date.Add(match.OraInizio);
                var dataFine = match.DataInizio.Date.Add(match.OraFine);

                // Imposta lo stato e la chat
                match.Stato = StatoPartita.InProgramma;
                match.Chat = new Chats { DataCreazione = DateTime.Now };

                if (dataFine <= match.DataInizio)
                {
                    return BadRequest("L'ora di fine deve essere successiva all'ora di inizio.");
                }

                // Aggiungi la partita al database
                _context.Matches.Add(match);
                await _context.SaveChangesAsync();

                // Crea una prenotazione per il creatore
                var booking = new Bookings
                {
                    DataPrenotazione = DateTime.Now,
                    StatoPrenotazione = StatoPrenotazione.InAttesa,
                    PagamentoEffettuato = false,
                    PartitaId = match.Id,
                    UtenteId = userId
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Reindirizza alla pagina di pagamento per la partita
                return RedirectToAction("BookAndPay", "Booking", new { id = match.Id });
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
                    .Where(m => m.CreatoreId == userId) // Partite create dall'utente loggato
                    .ToListAsync();

                // Recupera le partite create da altri utenti (non create dall'utente loggato)
                var otherMatches = await _context.Matches
                    .Include(m => m.Campo)
                    .Include(m => m.Partecipanti)
                    .Where(m => m.CreatoreId != userId) // Partite create da altri utenti
                    .ToListAsync();

                // Crea il ViewModel per passare entrambe le liste alla vista
                var viewModel = new MatchListViewModel
                {
                    UserMatches = userMatches,    // Partite create dall'utente
                    OtherMatches = otherMatches   // Partite create da altri utenti
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

                return View(match);
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

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verifica se l'utente è il creatore della partita
            if (match.CreatoreId != userId)
            {
                return Forbid("Non hai il permesso di modificare questa partita.");
            }

            var campi = _context.Fields.ToList();
            ViewBag.Campi = campi;

            return View(match);
        }

        // POST: Match/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditMatch(int id, Matches model)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verifica se l'utente è il creatore della partita
            if (match.CreatoreId != userId)
            {
                return Forbid("Non hai il permesso di modificare questa partita.");
            }

            match.DataInizio = model.DataInizio;
            match.OraInizio = model.OraInizio;
            match.OraFine = model.OraFine;
            match.TipoPartita = model.TipoPartita;
            match.CampoId = model.CampoId;

            await _context.SaveChangesAsync();
            return RedirectToAction("MatchList");
        }

        // 6. DELETE: Annullare una partita (solo dal creatore)
        [HttpGet]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Campo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (match.CreatoreId != userId)
            {
                return Forbid("Non hai il permesso di eliminare questa partita.");
            }

            return View(match);
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

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (match.CreatoreId != userId)
            {
                return Forbid("Non hai il permesso di eliminare questa partita.");
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MatchList));
        }
    }
}
