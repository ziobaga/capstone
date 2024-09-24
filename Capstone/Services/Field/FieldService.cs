﻿using Capstone.Models;
using Capstone.Models.Context;
using Capstone.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Services.Field
{
    public class FieldService : IFieldService
    {
        private readonly DataContext _context;

        public FieldService(DataContext context)
        {
            _context = context;
        }

        public async Task<Fields> CreateFieldAsync(FieldViewModel model, int userId)
        {
            // Crea una nuova istanza di Fields e assegna i valori dal model
            var newField = new Fields
            {
                NomeCampo = model.NomeCampo,
                Indirizzo = model.Indirizzo,
                Città = model.Città,
                TipoCampo = model.TipoCampo,
                PrezzoOrario = model.PrezzoOrario,
                ImmagineCampo = model.ImmagineCampo,
                ValutazioneMedia = 0,  // Inizialmente la valutazione media è 0
                UserId = userId  // Usa l'ID passato dal controller
            };

            if (model.ImmagineCampoFile != null && model.ImmagineCampoFile.Length > 0)
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "fields");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = Path.GetFileName(model.ImmagineCampoFile.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                // Salva il file sul server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImmagineCampoFile.CopyToAsync(stream);
                }

                newField.ImmagineCampo = Path.Combine("uploads", "fields", fileName);

                // Aggiorna il percorso dell'immagine di profilo
                // model.ImmagineCampo = Path.Combine("uploads", fileName);
            }

            // Aggiungi il nuovo campo al contesto
            _context.Fields.Add(newField);

            // Salva le modifiche nel database
            await _context.SaveChangesAsync();

            // Restituisci il campo creato
            return newField;
        }

        public async Task<IEnumerable<Fields>> GetAllFieldsAsync()
        {
            return await _context.Fields.ToListAsync();
        }

        public async Task<Fields> GetFieldByIdAsync(int id)
        {
            return await _context.Fields.FindAsync(id);
        }

        public async Task<bool> DeleteFieldAsync(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return false;
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool FieldExists(int id)
        {
            return _context.Fields.Any(e => e.Id == id);
        }

        public async Task<bool> UpdateFieldAsync(Fields field)
        {
            var existingField = await _context.Fields.FindAsync(field.Id);
            if (existingField == null)
            {
                return false;
            }

            existingField.NomeCampo = field.NomeCampo;
            existingField.Indirizzo = field.Indirizzo;
            existingField.Città = field.Città;
            existingField.TipoCampo = field.TipoCampo;
            existingField.PrezzoOrario = field.PrezzoOrario;


            await _context.SaveChangesAsync();
            return true;
        }
    }
}