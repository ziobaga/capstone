using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.Enums;
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

        public FieldController(IFieldService fieldService)
        {
            _fieldService = fieldService;
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

        public async Task<IActionResult> CreateField(Fields model)
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
        public async Task<IActionResult> GetEditField(int id)
        {
            // Trova il campo esistente nel database
            var existingField = await _fieldService.GetFieldByIdAsync(id);
            if (existingField == null)
            {
                return NotFound();
            }

            // Recupera i valori dal form
            var nomeCampo = Request.Form["NomeCampo"].ToString();
            var indirizzo = Request.Form["Indirizzo"].ToString();
            var citta = Request.Form["Città"].ToString();
            var tipoCampoString = Request.Form["TipoCampo"].ToString();
            var prezzoOrarioString = Request.Form["PrezzoOrario"].ToString();

            // Parsing dei valori
            var tipoCampo = (TipoCampo)Enum.Parse(typeof(TipoCampo), tipoCampoString);  // Enum per il tipo di campo
            var prezzoOrario = decimal.Parse(prezzoOrarioString);  // Prezzo orario come decimal

            // Aggiorna le proprietà dell'entità esistente
            existingField.NomeCampo = nomeCampo;
            existingField.Indirizzo = indirizzo;
            existingField.Città = citta;
            existingField.TipoCampo = tipoCampo;
            existingField.PrezzoOrario = prezzoOrario;

            try
            {
                // Indica a EF che l'entità è stata modificata
                _context.Entry(existingField).State = EntityState.Modified;

                // Salva i cambiamenti
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_fieldService.FieldExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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
    }
}
