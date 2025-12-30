namespace PCBuddy_Backend.DTOs
{
    public record MemoryDetailsDto(
        int Id,
        string Name,
        decimal Price,
        string? Speed,
        string? Modules,
        double? PricePerGb,
        string? Color,
        double? FirstWordLatency,
        double? CasLatency,
        string? ImageUrl,
        string? ProductUrl,
        string Type = "Memory"
    );
}