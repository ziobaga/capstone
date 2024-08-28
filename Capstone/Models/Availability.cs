using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class Availability
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DataOraInizio { get; set; }
        public DateTime DataOraFine { get; set; }

        // Relazioni
        public int CampoId { get; set; }
        public Field Campo { get; set; }
    }
}
