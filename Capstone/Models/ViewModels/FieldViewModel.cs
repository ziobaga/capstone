using Capstone.Models.Enums;

namespace Capstone.Models.ViewModels
{
    public class FieldViewModel
    {
        public int FieldId { get; set; }

        public string NomeCampo { get; set; }
        public string Indirizzo { get; set; }
        public string? Città { get; set; }
        public TipoCampo TipoCampo { get; set; } // Enum per definire calcio a 5 o a 7
        public decimal PrezzoOrario { get; set; }


        public string? ImmagineCampo { get; set; }

        public IFormFile ImmagineCampoFile { get; set; }
    }
}
