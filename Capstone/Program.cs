using Capstone.Models.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Capstone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var conn = builder.Configuration.GetConnectionString("DB");
            builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(conn));

            //AUTH
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Register";
                });

            var app = builder.Build();

            // Configura le policy di autorizzazione
            builder.Services.AddAuthorization(options => {
                options.AddPolicy("AdminPolicy", policy =>
            policy.RequireClaim("Role", "Admin"));  // Solo utenti con claim "Role" = "Admin"

                options.AddPolicy("GestorePolicy", policy =>
                    policy.RequireClaim("Role", "Gestore"));  // Solo utenti con claim "Role" = "Gestore"

                options.AddPolicy("AdminOrGestorePolicy", policy =>
                    policy.RequireClaim("Role", "Admin", "Gestore"));  // Admin o Gestore
            });

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
