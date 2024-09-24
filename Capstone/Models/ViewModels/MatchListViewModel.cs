namespace Capstone.Models.ViewModels
{
    public class MatchListViewModel
    {
        public IEnumerable<Matches> MatchesPartecipate { get; set; }
        public IEnumerable<Matches> MatchesNonPartecipate { get; set; }
        public List<int> BookedMatchIds { get; set; }  // Lista degli ID delle partite a cui l'utente partecipa
    }
}
