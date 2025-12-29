using System;

namespace PCBuddy_Backend.Models
{
    public interface ITrackable
    {
        DateTime UpdatedAt { get; set; }
        bool IsDeleted { get; set; }
    }
}