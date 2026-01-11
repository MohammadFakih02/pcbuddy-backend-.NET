
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
            if (!context.PrebuiltPCs.Any())
            {
                Console.WriteLine(" Seeding Prebuilt PCs...");

                var adminUser = context.Users.FirstOrDefault(u => u.Role == Role.ADMIN);
                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        Username = "SystemBuilder",
                        Email = "builder@pcbuddy.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        Role = Role.ADMIN,
                        Name = "PCBuddy Expert"
                    };
                    context.Users.Add(adminUser);
                    context.SaveChanges();
                }

                var cpuHigh = context.Cpus.OrderByDescending(p => p.Price).FirstOrDefault();
                var cpuMid = context.Cpus.FirstOrDefault(p => p.Price < 300 && p.Price > 150);

                var gpuHigh = context.Gpus.OrderByDescending(p => p.Price).FirstOrDefault();
                var gpuMid = context.Gpus.FirstOrDefault(p => p.Price < 500 && p.Price > 200);

                var ram = context.Memory.FirstOrDefault();
                var mobo = context.Motherboards.FirstOrDefault();
                var psu = context.PowerSupplies.FirstOrDefault();
                var storage = context.Storages.FirstOrDefault();
                var pcCase = context.Cases.FirstOrDefault();

                if (cpuHigh != null && gpuHigh != null && pcCase != null)
                {
                    var builds = new List<PrebuiltPC>
                    {
                        new PrebuiltPC
                        {
                            Name = "The Ultimate Destroyer",
                            EngineerId = adminUser.Id,
                            CpuId = cpuHigh.Id,
                            GpuId = gpuHigh.Id,
                            MemoryId = ram?.Id,
                            MotherboardId = mobo?.Id,
                            PowerSupplyId = psu?.Id,
                            StorageId = storage?.Id,
                            CaseId = pcCase?.Id,
                            TotalPrice = (cpuHigh.Price ?? 0) + (gpuHigh.Price ?? 0) + 500,
                            Rating = 5.0,
                            ImageUrl = pcCase.ImageUrl,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new PrebuiltPC
                        {
                            Name = "Value Gaming Beast",
                            EngineerId = adminUser.Id,
                            CpuId = cpuMid?.Id ?? cpuHigh.Id,
                            GpuId = gpuMid?.Id ?? gpuHigh.Id,
                            MemoryId = ram?.Id,
                            MotherboardId = mobo?.Id,
                            PowerSupplyId = psu?.Id,
                            StorageId = storage?.Id,
                            CaseId = pcCase?.Id,
                            TotalPrice = (cpuMid?.Price ?? 0) + (gpuMid?.Price ?? 0) + 400,
                            Rating = 4.5,
                            ImageUrl = pcCase.ImageUrl,
                            UpdatedAt = DateTime.UtcNow
                        }
                    };

                    context.PrebuiltPCs.AddRange(builds);
                    context.SaveChanges();
                    Console.WriteLine($" Seeded {builds.Count} Prebuilt PCs");
                }
            }
        }
    }
}