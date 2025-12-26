namespace PCBuddy_Backend.DTOs
{
    public record GpuDetailsDto(
        int Id,
        string Name,
        decimal Price,
        string Chipset,
        int Vram,
        string MemoryType,
        int Tdp
    );

}
