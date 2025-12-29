namespace PCBuddy_Backend.DTOs
{
    public record GpuDto(
        int Id,
        string Name,
        decimal Price,
        bool IsDeleted
    );
}
