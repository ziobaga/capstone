using Microsoft.EntityFrameworkCore;

namespace Capstone.Models.Context
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldManager> FieldManagers { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DataContext(DbContextOptions<DataContext> opt) : base(opt)
        {

        }

        // Configurazione delle chiavi composite per UserRole
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Configurazione della relazione molti-a-molti tra User e Match
            modelBuilder.Entity<User>()
                .HasMany(u => u.PartitePartecipate)
                .WithMany(m => m.Partecipanti)
                .UsingEntity<Dictionary<string, object>>(
                    "UserMatch",
                    j => j.HasOne<Match>().WithMany().HasForeignKey("MatchId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));

            // Configurazione della relazione uno-a-molti tra User e Match (Creatore)
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Creatore)
                .WithMany(u => u.PartiteCreate)
                .HasForeignKey(m => m.CreatoreId)
                .OnDelete(DeleteBehavior.Restrict);  // Impedisce l'eliminazione a cascata

            // Relazione con Valutatore (User)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Valutatore)
                .WithMany(u => u.RecensioniLasciate)  // Utente che lascia la recensione
                .HasForeignKey(r => r.ValutatoreId)
                .OnDelete(DeleteBehavior.Restrict);  // Evita cicli di cancellazione

            // Relazione con ValutatoGiocatore (User)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.ValutatoGiocatore)
                .WithMany(u => u.RecensioniRicevute)  // Utente che riceve la recensione
                .HasForeignKey(r => r.ValutatoGiocatoreId)
                .OnDelete(DeleteBehavior.Cascade);  // Elimina recensioni se il giocatore è cancellato

            // Relazione con ValutatoCampo (Field)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.ValutatoCampo)
                .WithMany()
                .HasForeignKey(r => r.ValutatoCampoId)
                .OnDelete(DeleteBehavior.Cascade);  // Elimina recensioni se il campo è cancellato

            base.OnModelCreating(modelBuilder);
        }
    }
}
