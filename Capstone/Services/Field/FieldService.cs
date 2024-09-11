using Capstone.Models;
using Capstone.Models.Context;
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

        public async Task<Fields> CreateFieldAsync(Fields field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            await _context.Fields.AddAsync(field);
            await _context.SaveChangesAsync();
            return field;
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
