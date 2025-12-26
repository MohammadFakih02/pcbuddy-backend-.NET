namespace PCBuilderApi.Models
{
    public class Motherboard
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }
        public double? Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? Chipset { get; set; }
        public int? MemoryMax { get; set; }
        public string? MemoryType { get; set; }
        public int? MemorySlots { get; set; }
        public string? MemorySpeed { get; set; }
        public int? PcieX16Slots { get; set; }
        public string? M2Slots { get; set; }
        public int? Sata6Gbps { get; set; }
        public string? OnboardEthernet { get; set; }
        public int? Usb20Headers { get; set; }
        public int? Usb32Gen1Headers { get; set; }
        public int? Usb32Gen2Headers { get; set; }
        public int? Usb32Gen2x2Headers { get; set; }
        public string? WirelessNetworking { get; set; }
        public string? Socket { get; set; }
        public string? FormFactor { get; set; }
        public int UsageCount { get; set; } = 0;
    }
}