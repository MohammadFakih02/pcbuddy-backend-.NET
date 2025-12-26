namespace PCBuddy_Backend.Models
{
    public class PowerSupply
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }
        public double? Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? PartNumber { get; set; }
        public string? Type { get; set; }
        public string? Efficiency { get; set; }
        public int? Wattage { get; set; }
        public bool? Modular { get; set; }
        public string? Color { get; set; }
        public double? Length { get; set; }
        public int? Atx4PinConnectors { get; set; }
        public int? Eps8PinConnectors { get; set; }
        public int? Molex4PinConnectors { get; set; }
        public int? Pcie12PinConnectors { get; set; }
        public int? Pcie12Plus4Pin12VHPWRConnectors { get; set; }
        public int? Pcie6PinConnectors { get; set; }
        public int? Pcie6Plus2PinConnectors { get; set; }
        public int? Pcie8PinConnectors { get; set; }
        public int? SataConnectors { get; set; }
        public int UsageCount { get; set; } = 0;
    }
}