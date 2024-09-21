namespace Capstone.Models.ViewModels
{
    public class ReviewViewModel
    {
        public int MatchId { get; set; }
        public int CampoId { get; set; }
        public string NomeCampo { get; set; }
        public int Punteggio { get; set; } // Da 0 a 5
        public string Commento { get; set; }
    }
}