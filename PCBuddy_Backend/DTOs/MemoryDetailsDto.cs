namespace PCBuddy_Backend.DTOs
{
    public record MemoryDetailsDto(
        int Id,
        string Name,
        decimal Price,
        int Capacity,
        int Speed,
        string Type
    );

}
