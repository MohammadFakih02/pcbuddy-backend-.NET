namespace PCBuddy.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double? Memory { get; set; }
        public string? GraphicsCard { get; set; }
        public string? Cpu { get; set; }
        public double? FileSize { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}