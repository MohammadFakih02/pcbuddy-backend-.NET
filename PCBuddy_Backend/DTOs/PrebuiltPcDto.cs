namespace PCBuddy_Backend.DTOs
{
    public record PrebuiltPcDto(
        int Id,
        string Name,
        decimal Price,
        double Rating,
        string? ImageUrl,
        bool IsDeleted
    );
}