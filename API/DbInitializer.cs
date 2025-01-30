using Core.Entities;
using Infrastructure.Data;
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
                context.Database.Migrate();
                await StoreContextSeed.SeedAsync(context);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
