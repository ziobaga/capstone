using Capstone.Models.Auth;
using Capstone.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models
{
    public class Fields
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
        public int UserId { get; set; }
        public Users User { get; set; }
        public ICollection<Matches> Partite { get; set; } = [];
        public ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
    }
}
