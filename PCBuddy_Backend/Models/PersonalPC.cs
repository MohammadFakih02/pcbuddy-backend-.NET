namespace PCBuddy.Models
{
    public class PersonalPC
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string? BuildName { get; set; }
        public BuildStatus BuildStatus { get; set; } = BuildStatus.COMPLETED;
        public bool AddToProfile { get; set; } = false;
        public DateTime UpdatedAt { get; set; }
        public double? TotalPrice { get; set; } = 0;
        public double? Rating { get; set; }
        public int UsageCount { get; set; } = 0;

        public int? CpuId { get; set; }
        public Cpu? Cpu { get; set; }

        public int? GpuId { get; set; }
        public Gpu? Gpu { get; set; }

        public int? MemoryId { get; set; }
        public Memory? Memory { get; set; }

        public int? MotherboardId { get; set; }
        public Motherboard? Motherboard { get; set; }

        public int? PowerSupplyId { get; set; }
        public PowerSupply? PowerSupply { get; set; }

        public int? StorageId { get; set; }
        public Storage? Storage { get; set; }

        public int? StorageId2 { get; set; }
        public Storage? Storage2 { get; set; }

        public int? CaseId { get; set; }
        public Case? Case { get; set; }
    }
}