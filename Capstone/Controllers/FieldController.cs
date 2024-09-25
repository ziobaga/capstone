using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.ViewModels;
using Capstone.Services.Field;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Capstone.Controllers
{
    public class FieldController : Controller
    {
        private readonly IFieldService _fieldService;
        private readonly DataContext _context;


        public FieldController(IFieldService fieldService, DataContext dataContext)
        {
            _fieldService = fieldService;
            _context = dataContext;
        }

        // GET: Field/List
        public async Task<IActionResult> FieldList()
        {
            var fields = await _fieldService.GetAllFieldsAsync();
            return View(fields);
        }

        // GET: Field/Details/5
        public async Task<IActionResult> FieldDetails(int id)
        {
            var field = await _fieldService.GetFieldByIdAsync(id);
            if (field == null)
            {
                return NotFound();
            }
            return View(field);
        }

        // GET: Field/Create
        public IActionResult CreateField()
        {
            return View();
        }

        // POST: Field/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateField(FieldViewModel model)
        {
            // Ottieni l'ID dell'utente loggato
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Passa l'ID dell'utente al servizio
            var newField = await _fieldService.CreateFieldAsync(model, userId);

            return RedirectToAction(nameof(FieldList));
        }

        // GET: Field/Edit/5
        [HttpGet]  // Metodo GET per mostrare il form
        public async Task<IActionResult> EditField(int id)
        {
            var field = await _fieldService.GetFieldByIdAsync(id);
            if (field == null)
            {
                return NotFound();
            }
            return View(field);
        }

        // POST: Field/Edit/5
        [HttpPost]  // Metodo POST per salvare le modifiche
        public async Task<IActionResult> EditField(int id, Fields model)
        {
            // Trova il campo esistente nel database
            var existingField = await _fieldService.GetFieldByIdAsync(id);
            if (existingField == null)
            {
                return NotFound();
            }

            // Recupera e aggiorna le proprietà dell'entità esistente
            existingField.NomeCampo = model.NomeCampo;
            existingField.Indirizzo = model.Indirizzo;
            existingField.Città = model.Città;
            existingField.TipoCampo = model.TipoCampo;
            existingField.PrezzoOrario = model.PrezzoOrario;

            try
            {
                // Indica a EF che l'entità è stata modificata
                _context.Entry(existingField).State = EntityState.Modified;

                // Salva i cambiamenti
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verifica se l'entità esiste ancora nel database
                if (!_fieldService.FieldExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Reindirizza alla vista della lista dei campi
            return RedirectToAction(nameof(FieldList));
        }



        // GET: Field/Delete/5
        public async Task<IActionResult> DeleteField(int id)
        {
            var field = await _fieldService.GetFieldByIdAsync(id);
            if (field == null)
            {
                return NotFound();
            }
            return View(field);
        }

        // POST: Field/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _fieldService.DeleteFieldAsync(id);
            if (!result)
            {
                return NotFound();
            }

            // Reindirizza alla lista dei campi dopo la cancellazione
            return RedirectToAction(nameof(FieldList));
        }

        // public IActionResult FieldNear(string address)
        // {
        // Verifica che l'indirizzo non sia nullo
        //if (string.IsNullOrEmpty(address))
        //  {
        //     return NotFound("Indirizzo non fornito.");
        // }

        // Passa l'indirizzo alla vista tramite ViewBag
        //  ViewBag.Address = address;
        //   return View();
        //}

    }
}