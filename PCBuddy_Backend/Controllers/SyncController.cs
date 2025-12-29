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
        public async Task<ActionResult<SyncResponseDto>> GetReferenceData([FromQuery] DateTime? lastSync)
        {
            var result = await _syncService.GetReferenceDataAsync(lastSync);

            return Ok(result);
        }
    }
}