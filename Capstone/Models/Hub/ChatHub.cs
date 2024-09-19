using Capstone.Models;
using Capstone.Models.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class ChatHub : Hub
{
    private readonly DataContext _context;

    public ChatHub(DataContext context)
    {
        _context = context;
    }

    // Metodo chiamato dal client per inviare un messaggio
    public async Task SendMessage(string message, int matchId)
    {
        // Trova l'ID dell'utente connesso
        var userId = int.Parse(Context.UserIdentifier);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            // Se l'utente non esiste, termina l'esecuzione
            throw new HubException("Utente non trovato.");
        }

        // Trova la chat associata alla partita
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

        // Invia il messaggio a tutti i client connessi alla chat di quella partita
        await Clients.Group(matchId.ToString()).SendAsync("ReceiveMessage", user.Nome, message);
    }

    // Aggiunge l'utente alla chat della partita quando si connette
    public override async Task OnConnectedAsync()
    {
        var matchId = Context.GetHttpContext().Request.Query["matchId"];

        // Controlla se il matchId non è vuoto o nullo
        if (!string.IsNullOrEmpty(matchId))
        {
            // Aggiungi l'utente al gruppo di SignalR basato sull'ID della partita
            await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var matchId = Context.GetHttpContext().Request.Query["matchId"];

        // Controlla se il matchId non è vuoto o nullo
        if (!string.IsNullOrEmpty(matchId))
        {
            // Rimuovi l'utente dal gruppo di SignalR
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
