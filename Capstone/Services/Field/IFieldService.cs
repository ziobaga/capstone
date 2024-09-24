using Capstone.Models;
using Capstone.Models.ViewModels;

namespace Capstone.Services.Field
{
    public interface IFieldService
    {
        Task<Fields> GetFieldByIdAsync(int id);
        Task<IEnumerable<Fields>> GetAllFieldsAsync();
        Task<Fields> CreateFieldAsync(FieldViewModel model, int userId);
        Task<bool> UpdateFieldAsync(Fields field);
        Task<bool> DeleteFieldAsync(int id);
        public bool FieldExists(int id);
    }
}