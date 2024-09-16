using Capstone.Models.Auth;
using Capstone.Models.Context;
using Capstone.Models.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace Capstone.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _ctx;
        private readonly IPasswordHelper _passwordHelper;

        public AuthService(DataContext dataContext, IPasswordHelper passwordHelper)
        {
            _ctx = dataContext;
            _passwordHelper = passwordHelper;

        }

        // Metodo per registrare un nuovo utente
        public async Task<Users> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var existingUser = await _ctx.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    throw new Exception("L'Username inserito è già in uso!");
                }

                // Hash della password
                if (string.IsNullOrEmpty(model.Password))
                {
                    throw new Exception("La password non può essere vuota.");
                }

                var userRegister = new Users
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = string.Empty,
                    Nome = model.Nome,
                    Cognome = model.Cognome,
                    DataCreazione = DateTime.Now
                };

                userRegister.PasswordHash = _passwordHelper.HashPassword(model.Password);

                // Assegna il ruolo all'utente tramite la tabella UserRoles
                var userRole = await _ctx.Roles.FirstOrDefaultAsync(r => r.Id == 3); // Ruolo User
                if (userRole == null)
                {
                    throw new Exception("Ruolo non trovato");
                }

                userRegister.Role.Add(userRole!);

                await _ctx.Users.AddAsync(userRegister);
                await _ctx.SaveChangesAsync();

                return userRegister;
            }
            catch (Exception ex)
            {
                // Log l'errore
                Console.WriteLine($"Errore durante la registrazione: {ex.Message}");
                return null;
            }
        }
        //Metodo per sapere l'id di chi logga
        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _ctx.Users.FindAsync(userId);
        }

        // Metodo per il login di un utente
        public async Task<Users> LoginAsync(LoginViewModel user)
        {
            var hashedPass = _passwordHelper.HashPassword(user.Password);
            // Trova l'utente in base a username e password
            var existingUser = await _ctx.Users
                .Include(u => u.Role) // Carica i ruoli tramite la tabella di collegamento
                .Where(u => u.Email == user.Email && u.PasswordHash == hashedPass)
                .FirstOrDefaultAsync();

            if (existingUser == null)
            {
                throw new Exception("Utente non trovato.");
            }

            return existingUser;
        }
    }
}