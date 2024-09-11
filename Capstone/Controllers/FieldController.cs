using Capstone.Models;
using Capstone.Services.Field;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class FieldController : Controller
    {
        private readonly IFieldService _fieldService;

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

        public async Task<IActionResult> CreateField([Bind("NomeCampo,Indirizzo,Città,TipoCampo,PrezzoOrario")] Fields field)
        {
            if (ModelState.IsValid)
            {
                // Imposta l'utente corrente
                field.UserId = 1; // Prendi l'ID dell'utente loggato dal contesto di autenticazione
                field.ValutazioneMedia = 0.0m; // Inizializza la valutazione media

                await _fieldService.CreateFieldAsync(field);
                return RedirectToAction(nameof(FieldList));
            }
            return View(field);
        }

        // GET: Field/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditField(int id, [Bind("Id,NomeCampo,Indirizzo,Città,TipoCampo,PrezzoOrario,ValutazioneMedia")] Fields field)
        {
            if (id != field.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _fieldService.UpdateFieldAsync(field);
                if (!result)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(FieldList));
            }
            return View(field);
        }

        // GET: Field/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var field = await _fieldService.GetFieldByIdAsync(id);
            if (field == null)
            {
                return NotFound();
            }
            return View(field);
        }

        // POST: Field/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _fieldService.DeleteFieldAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(FieldList));
        }
    }
}
