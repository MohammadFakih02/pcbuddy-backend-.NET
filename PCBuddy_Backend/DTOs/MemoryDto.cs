namespace PCBuddy_Backend.DTOs
{
    public record MemoryDto(
        int Id,
        string Name,
        decimal Price,
        string? ImageUrl,
        bool IsDeleted
    );
}
