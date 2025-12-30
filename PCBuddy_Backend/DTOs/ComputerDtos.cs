using PCBuddy_Backend.Models;

namespace PCBuddy_Backend.DTOs
{
    public record SavePCRequest(
        int? CpuId,
        int? GpuId,
        int? MemoryId,
        int? StorageId,
        int? StorageId2,
        int? MotherboardId,
        int? PowerSupplyId,
        int? CaseId,
        bool AddToProfile = false,
        double? Rating = null
    );

    public class SystemPartsResponse
    {
        public object? Cpu { get; set; }
        public object? Gpu { get; set; }
        public object? Memory { get; set; }
        public object? Storage { get; set; }
        public object? Storage2 { get; set; }
        public object? Motherboard { get; set; }
        public object? PowerSupply { get; set; }
        public object? Case { get; set; }
    }

    public record GameSearchResponse(
        List<Game> Games,
        int Total,
        int Page,
        int Limit
    );
}