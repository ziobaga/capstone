namespace Capstone.Models.ViewModels
{
    public class EditProfileViewModel
    {
        public int UserId { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Residenza { get; set; }
        public string ImmagineProfilo { get; set; }

        // File di immagine caricato dall'utente
        public IFormFile ImmagineProfiloFile { get; set; }
    }
}
