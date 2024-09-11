using Capstone.Models.Auth;

namespace Capstone.Services.Auth
{
    public interface IAuthService
    {
        Task<Users> RegisterAsync(Users user);
        Task<Users> LoginAsync(Users user);
    }
}
