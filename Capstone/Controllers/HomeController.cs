using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Ottieni l'ID dell'utente autenticato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Recupera l'utente dal database
            var user = await _context.Users
         .Include(u => u.PartiteCreate) // Include le partite create
         .Include(u => u.Prenotazioni)  // Include le prenotazioni dell'utente
         .ThenInclude(p => p.Partita)   // Include le partite associate alle prenotazioni
         .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("Utente non trovato.");
            }

            // Verifica se il profilo è completo (es. Residenza o ImmagineProfilo mancanti)
            bool profiloIncompleto = string.IsNullOrEmpty(user.Residenza) || string.IsNullOrEmpty(user.ImmagineProfilo);

            // Verifica le partite create dall'utente
            var partiteCreate = await _context.Matches
                .Where(m => m.CreatoreId == userId)
                .Include(m => m.Campo)
                .ToListAsync();

            // Verifica le partite a cui l'utente ha partecipato
            var partitePartecipate = await _context.Bookings
                .Where(b => b.UtenteId == userId && b.StatoPrenotazione == StatoPrenotazione.Confermata)
                .Include(b => b.Partita)
                .ThenInclude(p => p.Campo)
                .ToListAsync();

            // Passa entrambe le liste alla view tramite ViewData o un ViewModel
            var model = new HomeViewModel
            {
                PartiteCreate = partiteCreate,
                PartitePartecipate = partitePartecipate,
                ProfiloIncompleto = profiloIncompleto
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
