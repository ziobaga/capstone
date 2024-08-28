using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Capstone.Models.Enums;

namespace Capstone.Models
{
    public class Booking
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DataPrenotazione { get; set; }
        public StatoPrenotazione StatoPrenotazione { get; set; } // Enum per stato prenotazione
        public bool PagamentoEffettuato { get; set; }

        // Relazioni
        public int PartitaId { get; set; }
        public Match Partita { get; set; }

        public int UtenteId { get; set; }
        public User Utente { get; set; }
    }
}
