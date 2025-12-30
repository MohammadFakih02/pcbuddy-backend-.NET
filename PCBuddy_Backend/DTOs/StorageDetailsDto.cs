namespace PCBuddy_Backend.DTOs
{
    public record StorageDetailsDto(
        int Id,
        string Name,
        decimal Price,
        double? Capacity,
        string? Type,
        string? Cache,
        string? FormFactor,
        string? ImageUrl,
        string? ProductUrl,
        string PartType = "Storage"
    );
}