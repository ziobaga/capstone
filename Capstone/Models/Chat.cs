using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class Chat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DataCreazione { get; set; }

        // Relazioni
        public int PartitaId { get; set; }
        public Match Partita { get; set; }

        public ICollection<Message> Messaggi { get; set; }
    }
}
