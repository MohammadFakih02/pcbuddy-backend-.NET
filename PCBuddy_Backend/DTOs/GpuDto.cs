namespace PCBuddy_Backend.DTOs
{
    public record GpuDto(
        int Id,
        string Name,
        decimal Price,
        string? ImageUrl,
        bool IsDeleted
    );
}
