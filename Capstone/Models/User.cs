using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Capstone.Models.Enums;

namespace Capstone.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        [StringLength(20)]
        public string Cognome { get; set; }

        [Required]
        [StringLength(20)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public RuoloPreferito RuoloPreferito { get; set; }
        public decimal ValutazioneMedia { get; set; }
        public int PartiteGiocate { get; set; }
        public DateTime DataCreazione { get; set; }

        // Relazioni
        public ICollection<Booking> Prenotazioni { get; set; }
        public ICollection<Review> RecensioniLasciate { get; set; }
        public ICollection<Review> RecensioniRicevute { get; set; }  // Rinomina per chiarezza
        public ICollection<Message> MessaggiInviati { get; set; }

        public ICollection<Match> PartiteCreate { get; set; }  // Per partite create dall'utente
        public ICollection<Match> PartitePartecipate { get; set; } = new List<Match>(); // Per partite a cui l'utente partecipa
    }

}

