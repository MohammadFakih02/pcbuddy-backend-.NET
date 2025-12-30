namespace PCBuddy_Backend.DTOs
{
    public record GpuDetailsDto(
        int Id,
        string Name,
        decimal Price,
        string? Chipset,
        double? Memory,
        string? MemoryType,
        string? CoreClock,
        string? BoostClock,
        double? Length,
        int? Tdp,
        string? ImageUrl,
        string? ProductUrl,
        string Type = "Gpu"
    );
}