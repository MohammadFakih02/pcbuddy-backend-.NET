namespace PCBuddy_Backend.Models
{
    public class Storage:ITrackable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }
        public double? Price { get; set; }
        public string? Manufacturer { get; set; }
        public double? Capacity { get; set; }
        public string? Cache { get; set; }
        public string? FormFactor { get; set; }
        public int UsageCount { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}