﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class Chats
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DataCreazione { get; set; }

        // Relazioni
        public int PartitaId { get; set; }
        public Matches Partita { get; set; }

        public ICollection<Messages> Messaggi { get; set; }
    }
}
