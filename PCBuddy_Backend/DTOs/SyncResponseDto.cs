namespace PCBuddy_Backend.DTOs
{
    public record SyncResponseDto(
        List<CpuDto> Cpus,
        List<GpuDto> Gpus,
        List<MemoryDto> Memories,
        List<StorageDto> Storages,
        List<MotherboardDto> Motherboards,
        List<PowerSupplyDto> PowerSupplies,
        List<CaseDto> Cases,
        List<GameSyncDto> Games,
        string version
    );
}
