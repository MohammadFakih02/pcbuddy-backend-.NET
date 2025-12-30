namespace PCBuddy_Backend.DTOs
{
    public record PowerSupplyDetailsDto(
        int Id,
        string Name,
        decimal Price,
        int? Wattage,
        string? Type,
        string? Efficiency,
        bool? Modular,
        string? Color,
        string? ImageUrl,
        string? ProductUrl,
        string PartType = "PowerSupply"
    );
}