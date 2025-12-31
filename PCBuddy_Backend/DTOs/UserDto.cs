using PCBuddy_Backend.Models;

namespace PCBuddy_Backend.DTOs
{
    public record UserProfileDto(
        int Id,
        string Username,
        string Email,
        string? Name,
        string? Bio,
        string? ProfilePicture,
        Role Role,
        DateTime CreatedAt
    );

    public record UpdateProfileDto(
        string? Name,
        string? Bio
    );
}