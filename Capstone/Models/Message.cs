using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class Message
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Testo { get; set; }
        public DateTime DataInvio { get; set; }

        // Relazioni
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public int MittenteId { get; set; }
        public User Mittente { get; set; }
    }
}
