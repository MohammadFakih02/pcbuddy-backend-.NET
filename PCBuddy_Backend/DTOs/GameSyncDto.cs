namespace PCBuddy_Backend.DTOs
{
    public record GameSyncDto(
        int Id,
        string Name,
        string? MinCpu,
        string? MinGpu,
        decimal? MinMemoryGb,
        decimal? RequiredStorageGb
    );
}
