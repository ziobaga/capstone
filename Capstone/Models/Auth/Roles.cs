using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Auth
{
    public class Roles
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string NomeRuolo { get; set; }

        public List<Users> Users { get; set; } = [];
    }
}
