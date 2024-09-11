using Capstone.Models.Auth;

namespace Capstone.Services.User
{
    public class UserService : IUserService
    {
        public Task<Users> CreateUserAsync(Users user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(Users user)
        {
            throw new NotImplementedException();
        }
    }
}
