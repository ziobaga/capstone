using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.Enums;
using Capstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class ReviewsController : Controller
{
    private readonly DataContext _context;

    public ReviewsController(DataContext context)
    {
        _context = context;
    }

    // GET: Reviews/Create
    public async Task<IActionResult> CreateReview(int matchId)
    {
        var match = await _context.Matches
            .Include(m => m.Campo)
            .FirstOrDefaultAsync(m => m.Id == matchId);

        if (match == null)
        {
            return NotFound("Partita non trovata.");
        }

        var model = new ReviewViewModel
        {
            MatchId = matchId,
            CampoId = match.Campo.Id,
            NomeCampo = match.Campo.NomeCampo
        };

        return View(model);
    }

    // POST: Reviews/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateReview(ReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var review = new Reviews
            {
                Punteggio = model.Punteggio,
                Commento = model.Commento,
                DataRecensione = DateTime.Now,
                TipoRecensione = TipoRecensione.Campo,
                ValutatoreId = userId,
                ValutatoCampoId = model.CampoId
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Matches", new { id = model.MatchId });
        }

        return View(model);
    }
}
