namespace PCBuddy_Backend.Utils
{
    public static class FileUtils
    {
        // Allowed extensions for security
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        public static async Task<string> SaveProfilePictureAsync(IFormFile file, string webRootPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Only JPG, PNG, and WebP are allowed.");

            var uploadDir = Path.Combine(webRootPath, "uploads", "profiles");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            return $"/uploads/profiles/{fileName}";
        }

        public static void DeleteFile(string? relativePath, string webRootPath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;
            var cleanPath = relativePath.TrimStart('/', '\\');
            var fullPath = Path.Combine(webRootPath, cleanPath);

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file {fullPath}: {ex.Message}");
                }
            }
        }
    }
}