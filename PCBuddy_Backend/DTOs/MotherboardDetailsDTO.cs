namespace PCBuddy_Backend.DTOs
{
    public record MotherboardDetailsDto(
        int Id,
        string Name,
        decimal Price,
        string Socket,
        string Chipset,
        string FormFactor
    );

}
