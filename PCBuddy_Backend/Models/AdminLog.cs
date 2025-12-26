namespace PCBuddy.Models
{
    public class AdminLog
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;

        public int AdminId { get; set; }
        public User Admin { get; set; } = null!;

        public int? UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}