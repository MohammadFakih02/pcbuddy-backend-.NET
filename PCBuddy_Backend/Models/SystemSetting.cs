using System.ComponentModel.DataAnnotations;

namespace PCBuddy_Backend.Models
{
    public class SystemSetting
    {
        [Key]
        [MaxLength(50)]
        public string SettingKey { get; set; } = string.Empty;

        [MaxLength(255)]
        public string SettingValue { get; set; } = string.Empty;
    }
}