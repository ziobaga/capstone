namespace Capstone.Models.ViewModels
{
    public class AvailableMatchesViewModel
    {
        public IEnumerable<Matches> Matches { get; set; }
        public List<int> BookedMatchIds { get; set; } = new List<int>();// Elenco degli ID delle partite già prenotate
    }
}
