namespace Capstone.Models.ViewModels
{
    public class MatchListViewModel
    {
        public List<Matches> UserMatches { get; set; } // Partite create dall'utente loggato
        public List<Matches> OtherMatches { get; set; } // Partite create da altri utenti
    }
}
