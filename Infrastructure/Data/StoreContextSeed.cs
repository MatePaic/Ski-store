using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed<T> where T : class
    {
        public static async Task SeedAsync(StoreContext context, string jsonData)
        {
            if (!context.Set<T>().Any())
            {
                var data = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/" + jsonData);

                var entity = JsonSerializer.Deserialize<List<T>>(data);

                if (entity == null) return;

                context.Set<T>().AddRange(entity);

                await context.SaveChangesAsync();
            }
        }
    }
}
