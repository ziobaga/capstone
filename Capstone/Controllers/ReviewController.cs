using Capstone.Models;
using Capstone.Services.Review;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: Review/Create
        public IActionResult CreateReview(int campoId)
        {
            var model = new Reviews
            {
                ValutatoCampoId = campoId,
                DataRecensione = DateTime.Now
            };
            return View(model);
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview([Bind("Punteggio,Commento,ValutatoCampoId")] Reviews review)
        {
            if (ModelState.IsValid)
            {
                review.DataRecensione = DateTime.Now;
                review.ValutatoreId = int.Parse(User.FindFirst("UserId").Value); // Assumendo che l'ID dell'utente loggato sia nei claims
                await _reviewService.AddReviewAsync(review);
                return RedirectToAction("FieldDetails", "Field", new { id = review.ValutatoCampoId });
            }

            return View(review);
        }
    }
}
