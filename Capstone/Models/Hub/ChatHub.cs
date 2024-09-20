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

    // Metodo chiamato dal client per inviare un messaggio tramite SignalR
    public async Task SendMessage(int matchId, string message)
    {
        // Ottieni l'ID dell'utente connesso
        var userId = int.Parse(Context.UserIdentifier);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
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
        await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
        await base.OnConnectedAsync();
    }

    // Rimuove l'utente dalla chat della partita quando si disconnette
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var matchId = Context.GetHttpContext().Request.Query["matchId"];
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId);
        await base.OnDisconnectedAsync(exception);
    }
}
