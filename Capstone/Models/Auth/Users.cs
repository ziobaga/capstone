using Capstone.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Auth
{
    public class Users
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



        public RuoloPreferito RuoloPreferito { get; set; }
        public decimal ValutazioneMedia { get; set; }
        public int PartiteGiocate { get; set; }
        public DateTime DataCreazione { get; set; }

        // Relazioni
        public ICollection<Bookings> Prenotazioni { get; set; } = [];
        public ICollection<Reviews> RecensioniLasciate { get; set; } = [];
        public ICollection<Reviews> RecensioniRicevute { get; set; } = [];  // Rinomina per chiarezza
        public ICollection<Messages> MessaggiInviati { get; set; } = [];

        public ICollection<Matches> PartiteCreate { get; set; } = [];  // Per partite create dall'utente
        public ICollection<Matches> PartitePartecipate { get; set; } = []; // Per partite a cui l'utente partecipa

        public List<Roles> Role { get; set; } = [];
    }

}