using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.DTOs;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<UserProfileDto> GetProfile(int userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileDto(
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Name,
                    u.Bio,
                    // If AIUtils.FormatImageUrl handles adding "https://domain", keep it.
                    // Otherwise, just returning u.ProfilePicture is fine if it's a relative path.
                    AIUtils.FormatImageUrl(u.ProfilePicture),
                    u.Role,
                    u.CreatedAt
                ))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }

        public async Task<UserProfileDto> UpdateProfile(int userId, UpdateProfileDto updateData)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            if (updateData.Name != null) user.Name = updateData.Name;
            if (updateData.Bio != null) user.Bio = updateData.Bio;

            await _context.SaveChangesAsync();

            return new UserProfileDto(
                user.Id,
                user.Username,
                user.Email,
                user.Name,
                user.Bio,
                AIUtils.FormatImageUrl(user.ProfilePicture),
                user.Role,
                user.CreatedAt
            );
        }

        // Updated to handle IFormFile
        public async Task<UserProfileDto> UpdateProfilePicture(int userId, IFormFile file)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            // 1. Delete old image if it exists
            FileUtils.DeleteFile(user.ProfilePicture, _environment.WebRootPath);

            // 2. Save new image
            var relativePath = await FileUtils.SaveProfilePictureAsync(file, _environment.WebRootPath);

            // 3. Update DB
            user.ProfilePicture = relativePath;
            await _context.SaveChangesAsync();

            return new UserProfileDto(
                user.Id,
                user.Username,
                user.Email,
                user.Name,
                user.Bio,
                AIUtils.FormatImageUrl(user.ProfilePicture),
                user.Role,
                user.CreatedAt
            );
        }

        public async Task<int> GetUsersCount()
        {
            return await _context.Users.CountAsync();
        }
    }
}