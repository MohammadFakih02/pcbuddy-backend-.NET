using System.ComponentModel.DataAnnotations;

namespace PCBuddy_Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ProfilePicture { get; set; }
        public Role Role { get; set; } = Role.USER;
        public bool Banned { get; set; } = false;
        public string? Bio { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? Name { get; set; }

        public ICollection<PersonalPC> PersonalPCs { get; set; } = new List<PersonalPC>();
        public ICollection<PrebuiltPC> EngineeredPCs { get; set; } = new List<PrebuiltPC>();
        public ICollection<AdminLog> AdminLogs { get; set; } = new List<AdminLog>();
    }
}