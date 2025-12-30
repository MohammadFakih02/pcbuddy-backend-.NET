using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.DTOs;
using PCBuddy_Backend.Models;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Services
{
    public class ComputerService
    {
        private readonly AppDbContext _context;

        public ComputerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cpu>> GetCPUs() => await _context.Cpus.Select(x => new Cpu { Id = x.Id, Name = x.Name }).ToListAsync();
        public async Task<List<Gpu>> GetGPUs() => await _context.Gpus.Select(x => new Gpu { Id = x.Id, Name = x.Name, Chipset = x.Chipset }).ToListAsync();
        public async Task<List<Memory>> GetMemory() => await _context.Memory.Select(x => new Memory { Id = x.Id, Name = x.Name }).ToListAsync();
        public async Task<List<Storage>> GetStorage() => await _context.Storages.Select(x => new Storage { Id = x.Id, Name = x.Name, Type = x.Type, Capacity = x.Capacity }).ToListAsync();
        public async Task<List<Motherboard>> GetMotherboards() => await _context.Motherboards.Select(x => new Motherboard { Id = x.Id, Name = x.Name }).ToListAsync();
        public async Task<List<PowerSupply>> GetPowerSupplies() => await _context.PowerSupplies.Select(x => new PowerSupply { Id = x.Id, Name = x.Name, Wattage = x.Wattage }).ToListAsync();
        public async Task<List<Case>> GetCases() => await _context.Cases.Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();


        public async Task<decimal> CalculateTotalPrice(SavePCRequest parts)
        {
            double total = 0;

            if (parts.CpuId.HasValue) total += await _context.Cpus.Where(x => x.Id == parts.CpuId).Select(x => x.Price ?? 0).FirstOrDefaultAsync();
            if (parts.GpuId.HasValue) total += await _context.Gpus.Where(x => x.Id == parts.GpuId).Select(x => x.Price ?? 0).FirstOrDefaultAsync();
            if (parts.MemoryId.HasValue) total += await _context.Memory.Where(x => x.Id == parts.MemoryId).Select(x => x.Price ?? 0).FirstOrDefaultAsync();
            if (parts.StorageId.HasValue) total += await _context.Storages.Where(x => x.Id == parts.StorageId).Select(x => x.Price ?? 0).FirstOrDefaultAsync();
            if (parts.StorageId2.HasValue) total += await _context.Storages.Where(x => x.Id == parts.StorageId2).Select(x => x.Price ?? 0).FirstOrDefaultAsync();
            if (parts.MotherboardId.HasValue) total += await _context.Motherboards.Where(x => x.Id == parts.MotherboardId).Select(x => x.Price ?? 0).FirstOrDefaultAsync();
            if (parts.PowerSupplyId.HasValue) total += await _context.PowerSupplies.Where(x => x.Id == parts.PowerSupplyId).Select(x => x.Price ?? 0).FirstOrDefaultAsync();
            if (parts.CaseId.HasValue) total += await _context.Cases.Where(x => x.Id == parts.CaseId).Select(x => x.Price ?? 0).FirstOrDefaultAsync();

            return (decimal)total;
        }

        public async Task<PersonalPC> SavePCConfiguration(int userId, SavePCRequest parts)
        {
            var totalPrice = (double)await CalculateTotalPrice(parts);

            // Increment usage counts
            await IncrementUsageCount(parts);

            var existingPC = await _context.PersonalPCs.FirstOrDefaultAsync(p => p.UserId == userId);

            if (existingPC != null)
            {
                existingPC.CpuId = parts.CpuId;
                existingPC.GpuId = parts.GpuId;
                existingPC.MemoryId = parts.MemoryId;
                existingPC.StorageId = parts.StorageId;
                existingPC.StorageId2 = parts.StorageId2;
                existingPC.MotherboardId = parts.MotherboardId;
                existingPC.PowerSupplyId = parts.PowerSupplyId;
                existingPC.CaseId = parts.CaseId;
                existingPC.TotalPrice = totalPrice;
                existingPC.AddToProfile = parts.AddToProfile;
                if (parts.Rating.HasValue) existingPC.Rating = parts.Rating;

                existingPC.UpdatedAt = DateTime.UtcNow;
                _context.PersonalPCs.Update(existingPC);
                await _context.SaveChangesAsync();
                return existingPC;
            }
            else
            {
                var newPC = new PersonalPC
                {
                    UserId = userId,
                    CpuId = parts.CpuId,
                    GpuId = parts.GpuId,
                    MemoryId = parts.MemoryId,
                    StorageId = parts.StorageId,
                    StorageId2 = parts.StorageId2,
                    MotherboardId = parts.MotherboardId,
                    PowerSupplyId = parts.PowerSupplyId,
                    CaseId = parts.CaseId,
                    TotalPrice = totalPrice,
                    AddToProfile = parts.AddToProfile,
                    Rating = parts.Rating,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.PersonalPCs.Add(newPC);
                await _context.SaveChangesAsync();
                return newPC;
            }
        }

        private async Task IncrementUsageCount(SavePCRequest parts)
        {
            async Task UpdateCount<T>(DbSet<T> dbSet, int? id) where T : class
            {
                if (!id.HasValue) return;
                var entity = await dbSet.FindAsync(id.Value);
                if (entity != null)
                {
                    var prop = entity.GetType().GetProperty("UsageCount");
                    if (prop != null)
                    {
                        var current = (int)(prop.GetValue(entity) ?? 0);
                        prop.SetValue(entity, current + 1);
                    }
                }
            }

            await UpdateCount(_context.Cpus, parts.CpuId);
            await UpdateCount(_context.Gpus, parts.GpuId);
            await UpdateCount(_context.Memory, parts.MemoryId);
            await UpdateCount(_context.Storages, parts.StorageId);
            await UpdateCount(_context.Storages, parts.StorageId2);
            await UpdateCount(_context.Motherboards, parts.MotherboardId);
            await UpdateCount(_context.PowerSupplies, parts.PowerSupplyId);
            await UpdateCount(_context.Cases, parts.CaseId);

        }

        public async Task<PersonalPC> UpdatePCRating(int userId, int pcId, double rating)
        {
            var pc = await _context.PersonalPCs.FirstOrDefaultAsync(p => p.Id == pcId && p.UserId == userId);
            if (pc == null) throw new Exception("PC configuration not found for this user");

            pc.Rating = rating;
            pc.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return pc;
        }


        public async Task<SystemPartsResponse> GetPartDetails(SavePCRequest partIds)
        {
            var response = new SystemPartsResponse();

            if (partIds.CpuId.HasValue)
            {
                var p = await _context.Cpus.FindAsync(partIds.CpuId);
                if (p != null) response.Cpu = MapToCpuDetails(p);
            }

            if (partIds.GpuId.HasValue)
            {
                var p = await _context.Gpus.FindAsync(partIds.GpuId);
                if (p != null) response.Gpu = MapToGpuDetails(p);
            }

            if (partIds.MemoryId.HasValue)
            {
                var p = await _context.Memory.FindAsync(partIds.MemoryId);
                if (p != null) response.Memory = MapToMemoryDetails(p);
            }

            if (partIds.StorageId.HasValue)
            {
                var p = await _context.Storages.FindAsync(partIds.StorageId);
                if (p != null) response.Storage = MapToStorageDetails(p);
            }

            if (partIds.StorageId2.HasValue)
            {
                var p = await _context.Storages.FindAsync(partIds.StorageId2);
                if (p != null) response.Storage2 = MapToStorageDetails(p);
            }

            if (partIds.MotherboardId.HasValue)
            {
                var p = await _context.Motherboards.FindAsync(partIds.MotherboardId);
                if (p != null) response.Motherboard = MapToMotherboardDetails(p);
            }

            if (partIds.PowerSupplyId.HasValue)
            {
                var p = await _context.PowerSupplies.FindAsync(partIds.PowerSupplyId);
                if (p != null) response.PowerSupply = MapToPowerSupplyDetails(p);
            }

            if (partIds.CaseId.HasValue)
            {
                var p = await _context.Cases.FindAsync(partIds.CaseId);
                if (p != null) response.Case = MapToCaseDetails(p);
            }

            return response;
        }

        public async Task<SystemPartsResponse> GetUserPc(int userId)
        {
            var pc = await _context.PersonalPCs
                .Include(p => p.Cpu)
                .Include(p => p.Gpu)
                .Include(p => p.Memory)
                .Include(p => p.Storage)
                .Include(p => p.Storage2)
                .Include(p => p.Motherboard)
                .Include(p => p.PowerSupply)
                .Include(p => p.Case)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (pc == null) throw new Exception("No PC configuration found for this user");

            return new SystemPartsResponse
            {
                Cpu = pc.Cpu != null ? MapToCpuDetails(pc.Cpu) : null,
                Gpu = pc.Gpu != null ? MapToGpuDetails(pc.Gpu) : null,
                Memory = pc.Memory != null ? MapToMemoryDetails(pc.Memory) : null,
                Storage = pc.Storage != null ? MapToStorageDetails(pc.Storage) : null,
                Storage2 = pc.Storage2 != null ? MapToStorageDetails(pc.Storage2) : null,
                Motherboard = pc.Motherboard != null ? MapToMotherboardDetails(pc.Motherboard) : null,
                PowerSupply = pc.PowerSupply != null ? MapToPowerSupplyDetails(pc.PowerSupply) : null,
                Case = pc.Case != null ? MapToCaseDetails(pc.Case) : null
            };
        }


        public async Task<CpuDetailsDto?> GetCpuById(int id)
        {
            var p = await _context.Cpus.FindAsync(id);
            return p != null ? MapToCpuDetails(p) : null;
        }

        public async Task<GpuDetailsDto?> GetGpuById(int id)
        {
            var p = await _context.Gpus.FindAsync(id);
            return p != null ? MapToGpuDetails(p) : null;
        }

        public async Task<MemoryDetailsDto?> GetMemoryById(int id)
        {
            var p = await _context.Memory.FindAsync(id);
            return p != null ? MapToMemoryDetails(p) : null;
        }

        public async Task<StorageDetailsDto?> GetStorageById(int id)
        {
            var p = await _context.Storages.FindAsync(id);
            return p != null ? MapToStorageDetails(p) : null;
        }

        public async Task<MotherboardDetailsDto?> GetMotherboardById(int id)
        {
            var p = await _context.Motherboards.FindAsync(id);
            return p != null ? MapToMotherboardDetails(p) : null;
        }

        public async Task<PowerSupplyDetailsDto?> GetPowerSupplyById(int id)
        {
            var p = await _context.PowerSupplies.FindAsync(id);
            return p != null ? MapToPowerSupplyDetails(p) : null;
        }

        public async Task<CaseDetailsDto?> GetCaseById(int id)
        {
            var p = await _context.Cases.FindAsync(id);
            return p != null ? MapToCaseDetails(p) : null;
        }


        private CpuDetailsDto MapToCpuDetails(Cpu p) => new(
            p.Id, p.Name, (decimal)(p.Price ?? 0), p.CoreCount, p.PerformanceCoreClock,
            p.PerformanceCoreBoostClock, p.Socket, p.IntegratedGraphics, p.Series, p.Tdp,
            AIUtils.FormatImageUrl(p.ImageUrl), p.ProductUrl
        );

        private GpuDetailsDto MapToGpuDetails(Gpu p) => new(
            p.Id, $"{p.Name} {p.Chipset}", (decimal)(p.Price ?? 0), p.Chipset, p.Memory, p.MemoryType,
            p.CoreClock, p.BoostClock, p.Length, p.Tdp, AIUtils.FormatImageUrl(p.ImageUrl), p.ProductUrl
        );

        private MemoryDetailsDto MapToMemoryDetails(Memory p) => new(
            p.Id, p.Name, (decimal)(p.Price ?? 0), p.Speed, p.Modules, p.PricePerGb, p.Color,
            p.FirstWordLatency, p.CasLatency, AIUtils.FormatImageUrl(p.ImageUrl), p.ProductUrl
        );

        private StorageDetailsDto MapToStorageDetails(Storage p) => new(
            p.Id, $"{p.Name} {p.Capacity}GB", (decimal)(p.Price ?? 0), p.Capacity, p.Type, p.Cache,
            p.FormFactor, AIUtils.FormatImageUrl(p.ImageUrl), p.ProductUrl
        );

        private MotherboardDetailsDto MapToMotherboardDetails(Motherboard p) => new(
            p.Id, p.Name, (decimal)(p.Price ?? 0), p.Socket, p.Chipset, p.FormFactor, p.MemoryMax,
            p.MemorySlots, p.MemorySpeed, p.M2Slots, AIUtils.FormatImageUrl(p.ImageUrl), p.ProductUrl
        );

        private PowerSupplyDetailsDto MapToPowerSupplyDetails(PowerSupply p) => new(
            p.Id, p.Name, (decimal)(p.Price ?? 0), p.Wattage, p.Type, p.Efficiency, p.Modular,
            p.Color, AIUtils.FormatImageUrl(p.ImageUrl), p.ProductUrl
        );

        private CaseDetailsDto MapToCaseDetails(Case p) => new(
            p.Id, p.Name, (decimal)(p.Price ?? 0), p.Type, p.Color, p.SidePanel, p.MotherboardFormFactor,
            p.MaxVideoCardLength, p.DriveBays, AIUtils.FormatImageUrl(p.ImageUrl), p.ProductUrl
        );
    }
}