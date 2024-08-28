using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Capstone.Models.Enums;

namespace Capstone.Models
{
    public class Field
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string NomeCampo { get; set; }
        public string Indirizzo { get; set; }
        public string Città { get; set; }
        public TipoCampo TipoCampo { get; set; } // Enum per definire calcio a 5 o a 7
        public decimal PrezzoOrario { get; set; }
        public decimal ValutazioneMedia { get; set; }

        // Relazioni
        public int GestoreId { get; set; }
        public FieldManager Gestore { get; set; }

        public ICollection<Availability> Disponibilità { get; set; }
        public ICollection<Match> Partite { get; set; }
    }
}
