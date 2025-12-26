namespace PCBuddy_Backend.DTOs
{
    public record CpuDetailsDto(
        int Id,
        string Name,
        decimal Price,
        int Cores,
        int Threads,
        decimal BaseClock,
        decimal BoostClock,
        string Socket,
        int Tdp
    );

}
