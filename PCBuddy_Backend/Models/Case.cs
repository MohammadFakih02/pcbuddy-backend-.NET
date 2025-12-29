namespace PCBuddy_Backend.Models
{
    public class Case:ITrackable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }
        public double? Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? PartNumber { get; set; }
        public string? Type { get; set; }
        public string? Color { get; set; }
        public string? PowerSupply { get; set; }
        public string? SidePanel { get; set; }
        public bool? PowerSupplyShroud { get; set; }
        public string? FrontPanelUsb { get; set; }
        public string? MotherboardFormFactor { get; set; }
        public double? MaxVideoCardLength { get; set; }
        public string? DriveBays { get; set; }
        public string? ExpansionSlots { get; set; }
        public string? Dimensions { get; set; }
        public int UsageCount { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}