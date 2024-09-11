using Capstone.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models
{
    public class Messages
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Testo { get; set; }
        public DateTime DataInvio { get; set; }

        // Relazioni
        public int ChatId { get; set; }
        public Chats Chat { get; set; }

        public int MittenteId { get; set; }
        public Users Mittente { get; set; }
    }
}
