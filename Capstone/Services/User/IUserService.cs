using Capstone.Models.Auth;

namespace Capstone.Services.User
{
    public interface IUserService
    {
        Task<Users> GetUserByIdAsync(int id);
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> CreateUserAsync(Users user);
        Task<bool> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(int id);
    }
}
