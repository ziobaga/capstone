using Capstone.Models;

namespace Capstone.Services.Field
{
    public interface IFieldService
    {
        Task<Fields> GetFieldByIdAsync(int id);
        Task<IEnumerable<Fields>> GetAllFieldsAsync();
        Task<Fields> CreateFieldAsync(Fields field);
        Task<bool> UpdateFieldAsync(Fields field);
        Task<bool> DeleteFieldAsync(int id);
    }
}
