using Core.Entities;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed<T> where T : BaseEntity
    {
        public static async Task SeedAsync(StoreContext context, string jsonData)
        {
            if (!context.Set<T>().Any())
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var data = await File.ReadAllTextAsync(path + @"/Data/SeedData/" + jsonData);

                var entity = JsonSerializer.Deserialize<List<T>>(data);

                if (entity == null) return;

                context.Set<T>().AddRange(entity);

                await context.SaveChangesAsync();
            }
        }
    }
}
