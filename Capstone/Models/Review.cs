using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Capstone.Models.Enums;

namespace Capstone.Models
{
    public class Review
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Punteggio { get; set; }
        public string Commento { get; set; }
        public DateTime DataRecensione { get; set; }
        public TipoRecensione TipoRecensione { get; set; } // Enum per tipo di recensione (Campo, Giocatore)

        // Relazioni
        public int ValutatoreId { get; set; }
        public User Valutatore { get; set; }

        public int? ValutatoGiocatoreId { get; set; }  // Nullable per gestire la possibilità di null
        public User ValutatoGiocatore { get; set; }

        public int? ValutatoCampoId { get; set; }  // Nullable per gestire la possibilità di null
        public Field ValutatoCampo { get; set; }
    }

}
