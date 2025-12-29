namespace PCBuddy_Backend.DTOs
{
    public record PowerSupplyDto(
        int Id,
        string Name,
        decimal Price,
        bool IsDeleted
    );
}
