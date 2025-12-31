using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.DTOs;
using PCBuddy_Backend.Models;

namespace PCBuddy_Backend.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email || u.Username == request.Username))
            {
                throw new Exception("User with this email or username already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Name = request.Name,
                Password = passwordHash,
                Role = Role.USER,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = CreateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ProfilePicture = user.ProfilePicture
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new Exception("Invalid email or password.");
            }

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = CreateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ProfilePicture = user.ProfilePicture
            };
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT_KEY"] ?? throw new Exception("JWT Key not found in .env")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["JWT_ISSUER"],
                Audience = _configuration["JWT_AUDIENCE"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}