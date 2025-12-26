namespace PCBuddy_Backend.DTOs
{
    public record StorageDetailsDto(
        int Id,
        string Name,
        decimal Price,
        int Capacity,
        string Type,
        string Interface
    );

}
