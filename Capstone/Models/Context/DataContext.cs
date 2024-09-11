using Capstone.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Models.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Fields> Fields { get; set; }
       
        public DbSet<Matches> Matches { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Chats> Chats { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Roles> Roles { get; set; }
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
            modelBuilder.Entity<Users>()
                .HasMany(u => u.PartitePartecipate)
                .WithMany(m => m.Partecipanti)
                .UsingEntity<Dictionary<string, object>>(
                    "UserMatch",
                    j => j.HasOne<Matches>().WithMany().HasForeignKey("MatchId"),
                    j => j.HasOne<Users>().WithMany().HasForeignKey("UserId"));

            // Relazione tra Field e Users (Gestore)
            modelBuilder.Entity<Fields>()
                .HasOne(f => f.User)
                .WithMany()  // Un User può gestire più campi, ma non ha una proprietà di navigazione inversa
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Impedisce l'eliminazione a cascata

            // Configurazione della relazione uno-a-molti tra User e Match (Creatore)
            modelBuilder.Entity<Matches>()
                .HasOne(m => m.Creatore)
                .WithMany(u => u.PartiteCreate)
                .HasForeignKey(m => m.CreatoreId)
                .OnDelete(DeleteBehavior.Restrict);  // Impedisce l'eliminazione a cascata

            // Relazione con Valutatore (User)
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.Valutatore)
                .WithMany(u => u.RecensioniLasciate)  // Utente che lascia la recensione
                .HasForeignKey(r => r.ValutatoreId)
                .OnDelete(DeleteBehavior.Restrict);  // Evita cicli di cancellazione

            // Relazione con ValutatoGiocatore (User)
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.ValutatoGiocatore)
                .WithMany(u => u.RecensioniRicevute)  // Utente che riceve la recensione
                .HasForeignKey(r => r.ValutatoGiocatoreId)
                .OnDelete(DeleteBehavior.Restrict);  // Elimina recensioni se il giocatore è cancellato

            // Relazione con ValutatoCampo (Field)
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.ValutatoCampo)
                .WithMany()
                .HasForeignKey(r => r.ValutatoCampoId)
                .OnDelete(DeleteBehavior.Restrict);  // Elimina recensioni se il campo è cancellato

            base.OnModelCreating(modelBuilder);
        }
    }
}
