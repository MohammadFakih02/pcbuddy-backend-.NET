namespace PCBuddy_Backend.DTOs
{
    public record CaseDto(
        int Id,
        string Name,
        decimal Price,
        bool IsDeleted
    );
}
