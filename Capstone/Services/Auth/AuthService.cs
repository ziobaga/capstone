using Capstone.Models;
using Capstone.Models.Auth;
using Capstone.Models.Context;
using Microsoft.EntityFrameworkCore;


namespace Capstone.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _ctx;

        public AuthService(DataContext dataContext)
        {
            _ctx = dataContext;
        }

        // Metodo per registrare un nuovo utente
        public async Task<Users> RegisterAsync(Users user)
        {
            try
            {
                var existingUser = await _ctx.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (existingUser != null)
                {
                    throw new Exception("L'Username inserito è già in uso!");
                }

                // Hash della password
                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    throw new Exception("La password non può essere vuota.");
                }

                user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);

                // Aggiungi l'utente al contesto
                await _ctx.Users.AddAsync(user);
                await _ctx.SaveChangesAsync();

                // Assegna il ruolo all'utente tramite la tabella UserRoles
                var userRole = await _ctx.Roles.FirstOrDefaultAsync(r => r.Id == 1); // Ruolo Admin
                if (userRole == null)
                {
                    throw new Exception("Ruolo non trovato");
                }

                var userRoleLink = new UserRole
                {
                    UserId = user.Id,
                    RoleId = userRole.Id
                };

                await _ctx.UserRoles.AddAsync(userRoleLink);
                await _ctx.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                // Log l'errore
                Console.WriteLine($"Errore durante la registrazione: {ex.Message}");
                return null;
            }
        }

        // Metodo per il login di un utente
        public async Task<Users> LoginAsync(Users user)
        {
            // Hash della password in ingresso
            string hashedPassword = PasswordHelper.HashPassword(user.PasswordHash);

            // Trova l'utente in base a username e password
            var existingUser = await _ctx.Users
                .Include(u => u.UserRole) // Carica i ruoli tramite la tabella di collegamento
                .ThenInclude(ur => ur.Role) // Carica i dettagli dei ruoli
                .Where(u => u.Username == user.Username && u.PasswordHash == hashedPassword)
                .FirstOrDefaultAsync();

            if (existingUser == null)
            {
                throw new Exception("Username o password non corretti.");
            }

            return existingUser;
        }
    }
}
