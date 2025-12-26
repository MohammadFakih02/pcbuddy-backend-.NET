namespace PCBuddy_Backend.Models
{
    public class Cpu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }
        public double? Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? Series { get; set; }
        public string? Microarchitecture { get; set; }
        public string? Socket { get; set; }
        public int? CoreCount { get; set; }
        public double? PerformanceCoreClock { get; set; }
        public double? PerformanceCoreBoostClock { get; set; }
        public double? EfficiencyCoreClock { get; set; }
        public double? EfficiencyCoreBoostClock { get; set; }
        public string? L2Cache { get; set; }
        public string? L3Cache { get; set; }
        public int? Tdp { get; set; }
        public string? IntegratedGraphics { get; set; }
        public double? MaxSupportedMemory { get; set; }
        public bool? SimultaneousMultithreading { get; set; }
        public int UsageCount { get; set; } = 0;
    }
}