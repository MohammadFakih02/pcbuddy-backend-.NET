namespace PCBuddy_Backend.DTOs
{
    public record MotherboardDetailsDto(
        int Id,
        string Name,
        decimal Price,
        string? Socket,
        string? Chipset,
        string? FormFactor,
        int? MemoryMax,
        int? MemorySlots,
        string? MemorySpeed,
        string? M2Slots,
        string? ImageUrl,
        string? ProductUrl,
        string Type = "Motherboard"
    );
}