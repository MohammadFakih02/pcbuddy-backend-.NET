namespace PCBuddy_Backend.DTOs
{
    public record PrebuiltPcDto(
        int Id,
        string Name,
        decimal Price,
        double Rating,
        string? ImageUrl,
        bool IsDeleted,
        int? CpuId,
        int? GpuId,
        int? MemoryId,
        int? StorageId,
        int? MotherboardId,
        int? PowerSupplyId,
        int? CaseId
    );
}