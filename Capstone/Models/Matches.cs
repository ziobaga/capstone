using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Capstone.Models.Enums;
using Capstone.Models.Auth;

namespace Capstone.Models
{
    public class Matches
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DataInizio { get; set; }

        public DateTime DataFine { get; set; }
        public TipoPartita TipoPartita { get; set; } // Enum per calcio a 5 o a 7
        public StatoPartita Stato { get; set; } // Enum per stato partita

        // Relazioni
        public int CampoId { get; set; }
        public Fields Campo { get; set; }

        public ICollection<Users> Partecipanti { get; set; } = new List<Users>();

        public int CreatoreId { get; set; }
        public Users Creatore { get; set; }

        public ICollection<Bookings> Prenotazioni { get; set; }
        public Chats Chat { get; set; }
    }
}
