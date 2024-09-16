using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Capstone.Controllers
{

    public class MatchController : Controller
    {
        private readonly DataContext _context;

        public MatchController(DataContext dbContext)
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
                var matches = await _context.Matches
                    .Include(m => m.Campo)            // Includi i campi associati
                    .Include(m => m.Creatore)
                    .Include(m => m.Partecipanti)     // Includi i partecipanti
                    .Include(m => m.Chat)             // Includi le chat
                    .ToListAsync();

                return View(matches);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. GET: Recuperare i dettagli di una partita specifica
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            try
            {
                var match = await _context.Matches
                    .Include(m => m.Campo)
                    .Include(m => m.Partecipanti)
                    .Include(m => m.Chat)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (match == null)
                    return NotFound("Partita non trovata");

                return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Match/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> EditMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Campo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            // Recupera i campi disponibili per popolare il dropdown
            var campi = _context.Fields.ToList();
            ViewBag.Campi = campi;

            return View(match);
        }

        // 5. PUT: Modificare una partita creata dall'utente (solo dal creatore)
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditMatch(int id, [FromBody] Matches updatedMatch, [FromQuery] int userId)
        {
            try
            {
                var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);

                if (match == null)
                    return NotFound("Partita non trovata");

                // Controlla se l'utente è il creatore della partita
                if (match.CreatoreId != userId)
                    return Forbid("Non hai il permesso di modificare questa partita.");

                // Aggiorna i dettagli della partita
                match.DataInizio = updatedMatch.DataInizio;
                match.OraInizio = updatedMatch.OraInizio;
                match.OraFine = updatedMatch.OraFine;
                match.TipoPartita = updatedMatch.TipoPartita;
                match.CampoId = updatedMatch.CampoId;

                await _context.SaveChangesAsync();
                return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 6. DELETE: Annullare una partita (solo dal creatore)
        [HttpDelete("cancel/{id}")]
        public async Task<IActionResult> CancelMatch(int id, [FromQuery] int userId)
        {
            try
            {
                var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);

                if (match == null)
                    return NotFound("Partita non trovata");

                // Controlla se l'utente è il creatore della partita
                if (match.CreatoreId != userId)
                    return Forbid("Non hai il permesso di annullare questa partita.");

                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();

                return Ok("Partita annullata con successo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
