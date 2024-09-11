using Capstone.Models.Auth;
using Capstone.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models
{
    public class Reviews
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Punteggio { get; set; }
        public string Commento { get; set; }
        public DateTime DataRecensione { get; set; }
        public TipoRecensione TipoRecensione { get; set; } // Enum per tipo di recensione (Campo, Giocatore)

        // Relazioni
        public int ValutatoreId { get; set; }
        public Users Valutatore { get; set; }

        public int? ValutatoGiocatoreId { get; set; }  // Nullable per gestire la possibilità di null
        public Users ValutatoGiocatore { get; set; }

        public int? ValutatoCampoId { get; set; }  // Nullable per gestire la possibilità di null
        public Fields ValutatoCampo { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }
    }

}
