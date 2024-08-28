using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Capstone.Models.Enums;

namespace Capstone.Models
{
    public class Match
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DataOra { get; set; }
        public TipoPartita TipoPartita { get; set; } // Enum per calcio a 5 o a 7
        public StatoPartita Stato { get; set; } // Enum per stato partita

        // Relazioni
        public int CampoId { get; set; }
        public Field Campo { get; set; }

        public ICollection<User> Partecipanti { get; set; } = new List<User>();

        public int CreatoreId { get; set; }
        public User Creatore { get; set; }

        public ICollection<Booking> Prenotazioni { get; set; }
        public Chat Chat { get; set; }
    }
}
