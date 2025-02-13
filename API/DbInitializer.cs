using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class DbInitializer
    {
        public static async void InitDb(WebApplication application)
        {
            try
            {
                using var scope = application.Services.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<StoreContext>()
                    ?? throw new InvalidOperationException("Failed to retrieve store context");
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>()
                    ?? throw new InvalidOperationException("Failed to retrieve user manager");

                context.Database.Migrate();

                await SeedUserDataAsync(userManager);
                await StoreContextSeed<Product>.SeedAsync(context, "products.json");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private static async Task SeedUserDataAsync(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    FirstName = "bob",
                    LastName = "smith",
                    UserName = "bob@test.com",
                    Email = "bob@test.com"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");

                var admin = new User
                {
                    FirstName = "admin",
                    LastName = "smith",
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, ["Member", "Admin"]);
            }
        }
    }
}
