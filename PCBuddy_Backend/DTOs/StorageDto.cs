namespace PCBuddy_Backend.DTOs
{
    public record StorageDto(
        int Id,
        string Name,
        decimal Price,
        bool IsDeleted
    );
}
