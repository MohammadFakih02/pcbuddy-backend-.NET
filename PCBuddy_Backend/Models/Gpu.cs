namespace PCBuddy_Backend.Models
{
    public class Gpu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }
        public double? Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? PartNumber { get; set; }
        public string? Chipset { get; set; }
        public double? Memory { get; set; }
        public string? MemoryType { get; set; }
        public string? CoreClock { get; set; }
        public string? BoostClock { get; set; }
        public string? EffectiveMemoryClock { get; set; }
        public string? Color { get; set; }
        public string? FrameSync { get; set; }
        public double? Length { get; set; }
        public int? Tdp { get; set; }
        public int? CaseExpansionSlotWidth { get; set; }
        public int? TotalSlotWidth { get; set; }
        public string? ExternalPower { get; set; }
        public int UsageCount { get; set; } = 0;
    }
}