using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCBuddy_Backend.DTOs;
using PCBuddy_Backend.Services;
using System.Security.Claims;

namespace PCBuddy_Backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetMyProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var profile = await _userService.GetProfile(userId);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetProfile(int id)
        {
            try
            {
                var profile = await _userService.GetProfile(id);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<ActionResult<UserProfileDto>> UpdateProfile([FromBody] UpdateProfileDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var updatedProfile = await _userService.UpdateProfile(userId, request);
                return Ok(updatedProfile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Changed to HttpPost (common for uploads) or HttpPut
        // Accepts IFormFile from Form Data
        [Authorize]
        [HttpPost("profile-picture")]
        public async Task<ActionResult<UserProfileDto>> UpdateProfilePicture([FromForm] IFormFile file)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var updatedProfile = await _userService.UpdateProfilePicture(userId, file);
                return Ok(updatedProfile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetUsersCount()
        {
            return Ok(await _userService.GetUsersCount());
        }
    }
}