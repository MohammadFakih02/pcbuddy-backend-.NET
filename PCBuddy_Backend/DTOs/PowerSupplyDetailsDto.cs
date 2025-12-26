namespace PCBuddy_Backend.DTOs
{
    public record PowerSupplyDetailsDto(
        int Id,
        string Name,
        decimal Price,
        int Wattage,
        string EfficiencyRating,
        bool Modular
    );

}
