namespace PCBuddy_Backend.DTOs
{
    public record CaseDetailsDto(
        int Id,
        string Name,
        decimal Price,
        string? Type,
        string? Color,
        string? SidePanel,
        string? MotherboardFormFactor,
        double? MaxVideoCardLength,
        string? DriveBays,
        string? ImageUrl,
        string? ProductUrl,
        string PartType = "Case"
    );
}