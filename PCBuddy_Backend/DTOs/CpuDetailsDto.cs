namespace PCBuddy_Backend.DTOs
{
    public record CpuDetailsDto(
        int Id,
        string Name,
        decimal Price,
        int? CoreCount,
        double? PerformanceCoreClock,
        double? PerformanceCoreBoostClock,
        string? Socket,
        string? IntegratedGraphics,
        string? Series,
        int? Tdp,
        string? ImageUrl,
        string? ProductUrl,
        string Type = "Cpu"
    );
}