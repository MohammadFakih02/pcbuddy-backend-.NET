namespace PCBuddy_Backend.DTOs
{
    public record CaseDetailsDto(
        int Id,
        string Name,
        decimal Price,
        string FormFactor,
        int MaxGpuLength,
        int MaxCpuCoolerHeight
    );

}
