namespace PCBuddy_Backend.Models
{
    public class Memory:ITrackable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }
        public double? Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? Speed { get; set; }
        public string? Modules { get; set; }
        public double? PricePerGb { get; set; }
        public string? Color { get; set; }
        public double? FirstWordLatency { get; set; }
        public double? CasLatency { get; set; }
        public double? Voltage { get; set; }
        public string? Timing { get; set; }
        public bool? HeatSpreader { get; set; }
        public int UsageCount { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}