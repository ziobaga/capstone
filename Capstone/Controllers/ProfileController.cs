using Capstone.Models.Context;
using Capstone.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Capstone.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly DataContext _context;

        public ProfileController(DataContext context)
        {
            _context = context;
        }

        // GET: Profile/EditProfile
        public async Task<IActionResult> EditProfile()
        {
            // Ottieni l'ID dell'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Recupera l'utente dal database
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("Utente non trovato.");
            }

            // Mappa l'utente su EditProfileViewModel
            var model = new EditProfileViewModel
            {
                UserId = user.Id,
                Nome = user.Nome,
                Cognome = user.Cognome,
                Username = user.Username,
                Residenza = user.Residenza,
                Città = user.Città,
                ImmagineProfilo = user.ImmagineProfilo // Percorso dell'immagine profilo esistente
            };

            return View(model);
        }

        // POST: Profile/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {


            var user = await _context.Users.FindAsync(model.UserId);

            if (user == null)
            {
                return NotFound("Utente non trovato.");
            }

            // Aggiorna i campi dell'utente con i dati del ViewModel
            user.Nome = model.Nome;
            user.Cognome = model.Cognome;
            user.Username = model.Username;
            user.Residenza = model.Residenza;
            user.Città = model.Città;
            user.RuoloPreferito = model.RuoloPreferito;

            // Se l'utente ha caricato una nuova immagine di profilo
            if (model.ImmagineProfiloFile != null && model.ImmagineProfiloFile.Length > 0)
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = Path.GetFileName(model.ImmagineProfiloFile.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                // Salva il file sul server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImmagineProfiloFile.CopyToAsync(stream);
                }

                // Aggiorna il percorso dell'immagine di profilo
                user.ImmagineProfilo = Path.Combine("uploads", fileName);
            }

            // Salva le modifiche nel database
            await _context.SaveChangesAsync();

            // Reindirizza alla pagina dei dettagli del profilo
            return RedirectToAction("Details", new { id = user.Id });
        }



        // GET: Profile/Details
        public async Task<IActionResult> Details()
        {
            // Ottieni l'ID dell'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Recupera l'utente dal database
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("Utente non trovato.");
            }

            return View(user); // Mostra i dettagli del profilo
        }

        public async Task<IActionResult> ViewProfile(int userid)
        {
            var user = await _context.Users.FindAsync(userid);
            if (user == null)
            {
                return NotFound("Utente non trovato.");
            }
            return View(user); // Usa la stessa vista dei dettagli del profilo
        }
    }
}
