
using PCBuddy_Backend.Models;
using System.Text.Json;

namespace PCBuddy_Backend.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // 1. CPU
            if (!context.Cpus.Any())
            {
                var data = File.ReadAllText(Path.Combine(dataPath, "cpus.json"));
                var items = JsonSerializer.Deserialize<List<Cpu>>(data, options);
                if (items != null)
                {
                    items.ForEach(i => i.Id = 0);
                    context.Cpus.AddRange(items);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {items.Count} CPUs");
                }
            }

            // 2. GPU
            if (!context.Gpus.Any())
            {
                var data = File.ReadAllText(Path.Combine(dataPath, "gpus.json"));
                var items = JsonSerializer.Deserialize<List<Gpu>>(data, options);
                if (items != null)
                {
                    items.ForEach(i => i.Id = 0);
                    context.Gpus.AddRange(items);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {items.Count} GPUs");
                }
            }

            // 3. Case
            if (!context.Cases.Any())
            {
                var data = File.ReadAllText(Path.Combine(dataPath, "cases.json"));
                var items = JsonSerializer.Deserialize<List<Case>>(data, options);
                if (items != null)
                {
                    items.ForEach(i => i.Id = 0);
                    context.Cases.AddRange(items);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {items.Count} Cases");
                }
            }

            // 4. Memory
            if (!context.Memory.Any())
            {
                var data = File.ReadAllText(Path.Combine(dataPath, "memory.json"));
                var items = JsonSerializer.Deserialize<List<Memory>>(data, options);
                if (items != null)
                {
                    items.ForEach(i => i.Id = 0);
                    context.Memory.AddRange(items);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {items.Count} Memory modules");
                }
            }

            // 5. Storage
            if (!context.Storages.Any())
            {
                var data = File.ReadAllText(Path.Combine(dataPath, "storages.json"));
                var items = JsonSerializer.Deserialize<List<Storage>>(data, options);
                if (items != null)
                {
                    items.ForEach(i => i.Id = 0);
                    context.Storages.AddRange(items);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {items.Count} Storage devices");
                }
            }

            // 6. Power Supply
            if (!context.PowerSupplies.Any())
            {
                var data = File.ReadAllText(Path.Combine(dataPath, "power_supplies.json"));
                var items = JsonSerializer.Deserialize<List<PowerSupply>>(data, options);
                if (items != null)
                {
                    items.ForEach(i => i.Id = 0);
                    context.PowerSupplies.AddRange(items);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {items.Count} Power Supplies");
                }
            }

            // 7. Motherboard
            if (!context.Motherboards.Any())
            {
                var data = File.ReadAllText(Path.Combine(dataPath, "motherboards.json"));
                var items = JsonSerializer.Deserialize<List<Motherboard>>(data, options);
                if (items != null)
                {
                    items.ForEach(i => i.Id = 0);
                    context.Motherboards.AddRange(items);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {items.Count} Motherboards");
                }
            }

            // 8. Games
            if (!context.Games.Any())
            {
                var data = File.ReadAllText(Path.Combine(dataPath, "Video_Game_requirements.json"));
                var items = JsonSerializer.Deserialize<List<Game>>(data, options);
                if (items != null)
                {
                    items.ForEach(i => i.Id = 0);
                    context.Games.AddRange(items);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {items.Count} Games");
                }
            }
        }
    }
}