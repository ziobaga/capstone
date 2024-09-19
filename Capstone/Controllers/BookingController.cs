using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;

namespace Capstone.Controllers
{
    public class BookingController : Controller
    {
        private readonly DataContext _context;

        public BookingController(DataContext context)
        {
            _context = context;
            StripeConfiguration.ApiKey = "sk_test_51Q0mODP563WlEe7GsBqEQglmxgCiUBn1hXytJSGH76JropG9s1n8f3eXxsHlLZ8lrN9DG3L4BFvMhbhMWTxMyaTJ00vmj2MlP4";
        }

        // GET: Booking/BookAndPay/5
        [HttpGet]
        public async Task<IActionResult> BookAndPay(int id)
        {
            // Recupera la partita
            var match = await _context.Matches
                .Include(m => m.Campo)
                .Include(m => m.Partecipanti)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound("Partita non trovata");
            }

            // Passa i dettagli della partita alla vista
            return View(match);
        }

        // POST: Booking/ProcessPayment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int matchId)
        {
            var match = await _context.Matches
                .Include(m => m.Campo)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
            {
                return NotFound("Partita non trovata");
            }

            // Ottieni l'ID dell'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verifica se esiste già una prenotazione per questa partita
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.PartitaId == match.Id && b.UtenteId == userId);

            if (existingBooking != null)
            {
                return BadRequest("Hai già prenotato per questa partita.");
            }

            // Calcola la durata della partita in ore
            var durataPartita = (match.OraFine - match.OraInizio).TotalHours;

            // Ottieni il prezzo orario del campo (converti decimal in double)
            var prezzoOrario = (double)match.Campo.PrezzoOrario;

            // Calcola il totale da pagare (moltiplica la durata per il prezzo orario)
            var importoDaPagare = durataPartita * prezzoOrario * 100; // Converti in centesimi per Stripe

            // Creazione della prenotazione con stato "In Attesa di Pagamento"
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

            // Integrazione con Stripe per il pagamento
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = (long)importoDaPagare,
                    Currency = "eur",
                    Description = "Prenotazione per partita",
                    Source = "tok_visa", // Sostituisci con il token ottenuto da Stripe Checkout
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);

                if (charge.Status == "succeeded")
                {
                    // Aggiorna lo stato della prenotazione
                    booking.PagamentoEffettuato = true;
                    booking.StatoPrenotazione = StatoPrenotazione.Confermata;
                    await _context.SaveChangesAsync();

                    // Verifica se esiste già una chat per la partita
                    var chat = await _context.Chats.FirstOrDefaultAsync(c => c.PartitaId == match.Id);
                    if (chat == null)
                    {
                        // Crea una nuova chat se non esiste
                        chat = new Chats { PartitaId = match.Id, DataCreazione = DateTime.Now };
                        _context.Chats.Add(chat);
                        await _context.SaveChangesAsync();
                    }

                    // Passa il modello di tipo `Chats` alla vista, non `Matches`
                    return RedirectToAction("ForMatch", "Chat", new { matchId = match.Id });
                }
                else
                {
                    return BadRequest("Il pagamento non è riuscito.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore durante il pagamento: {ex.Message}");
            }
        }

    }
}
