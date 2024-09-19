using Capstone.Models;
using Capstone.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Capstone.Controllers
{
    public class ChatController : Controller
    {
        private readonly DataContext _context;
        private readonly IHubContext<ChatHub> _chatHub;

        public ChatController(DataContext context, IHubContext<ChatHub> chatHub)
        {
            _context = context;
            _chatHub = chatHub;
        }

        // GET: Chat/ForMatch/5
        // Recupera tutti i messaggi di una partita specifica
        public async Task<IActionResult> ForMatch(int matchId)
        {
            // Recupera la chat associata alla partita
            var chat = await _context.Chats
                .Include(c => c.Messaggi)
                .ThenInclude(m => m.Mittente)
                .FirstOrDefaultAsync(c => c.PartitaId == matchId);

            if (chat == null)
            {
                return NotFound("Nessuna chat trovata per questa partita.");
            }

            // Passa il modello di tipo `Chats` alla vista
            return View(chat);
        }

        // POST: Chat/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessage(int matchId, string message)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Ottieni l'ID dell'utente connesso
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Unauthorized("Utente non autorizzato.");
            }

            // Trova la chat della partita
            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.PartitaId == matchId);
            if (chat == null)
            {
                // Se non esiste una chat per la partita, creane una
                chat = new Chats { PartitaId = matchId, DataCreazione = DateTime.Now };
                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
            }

            // Crea un nuovo messaggio
            var newMessage = new Messages
            {
                Testo = message,
                DataInvio = DateTime.Now,
                ChatId = chat.Id,
                MittenteId = userId
            };

            // Salva il messaggio nel database
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Invia il messaggio a tutti i client connessi alla chat di quella partita tramite SignalR
            await _chatHub.Clients.Group(matchId.ToString()).SendAsync("ReceiveMessage", user.Nome, message);

            return Ok(); // Risposta di successo
        }
    }
}
