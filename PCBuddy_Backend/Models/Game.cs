namespace PCBuddy_Backend.Models
{
    public class Game:ITrackable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double? Memory { get; set; }
        public string? GraphicsCard { get; set; }
        public string? Cpu { get; set; }
        public double? FileSize { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}