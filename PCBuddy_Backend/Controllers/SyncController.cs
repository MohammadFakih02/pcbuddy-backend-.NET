using Microsoft.AspNetCore.Mvc;
using PCBuddy_Backend.DTOs;
using PCBuddy_Backend.Services;

namespace PCBuddy.Controllers
{
    [ApiController]
    [Route("api/sync")]
    public class SyncController : ControllerBase
    {
        private readonly SyncService _syncService;

        public SyncController(SyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet("reference-data")]
        public async Task<ActionResult<SyncResponseDto>> GetReferenceData([FromQuery] string? currentVersion)
        {
            var result = await _syncService.GetReferenceDataAsync(currentVersion);

            if (result == null)
            {
                return StatusCode(304);
            }

            return Ok(result);
        }
    }
}