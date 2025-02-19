﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Auth
{
    public class Roles
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string NomeRuolo { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
