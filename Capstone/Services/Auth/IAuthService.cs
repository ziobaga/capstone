using Capstone.Models.Auth;
using Capstone.Models.ViewModels;

namespace Capstone.Services.Auth
{
    public interface IAuthService
    {
        Task<Users> RegisterAsync(RegisterViewModel user);
        Task<Users> LoginAsync(LoginViewModel user);

        Task<Users> GetUserByIdAsync(int userId);
    }
}
