using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCBuddy_Backend.DTOs;
using PCBuddy_Backend.Services;
using System.Security.Claims;

namespace PCBuddy_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/computer")]
    public class ComputerController : ControllerBase
    {
        private readonly ComputerService _computerService;

        public ComputerController(ComputerService computerService)
        {
            _computerService = computerService;
        }

        [HttpGet("cpus")]
        public async Task<IActionResult> GetCpus() => Ok(await _computerService.GetCPUs());

        [HttpGet("gpus")]
        public async Task<IActionResult> GetGpus() => Ok(await _computerService.GetGPUs());

        [HttpGet("memory")]
        public async Task<IActionResult> GetMemory() => Ok(await _computerService.GetMemory());

        [HttpGet("storage")]
        public async Task<IActionResult> GetStorage() => Ok(await _computerService.GetStorage());

        [HttpGet("motherboards")]
        public async Task<IActionResult> GetMotherboards() => Ok(await _computerService.GetMotherboards());

        [HttpGet("powersupplies")]
        public async Task<IActionResult> GetPowerSupplies() => Ok(await _computerService.GetPowerSupplies());

        [HttpGet("cases")]
        public async Task<IActionResult> GetCases() => Ok(await _computerService.GetCases());

        [HttpPost("calculate-price")]
        public async Task<IActionResult> CalculatePrice([FromBody] SavePCRequest request)
        {
            var price = await _computerService.CalculateTotalPrice(request);
            return Ok(new { TotalPrice = price });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveConfiguration([FromBody] SavePCRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (userId <= 0) return Unauthorized();

                var result = await _computerService.SavePCConfiguration(userId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("rate")]
        public async Task<IActionResult> RatePc([FromQuery] int pcId, [FromQuery] double rating)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var result = await _computerService.UpdatePCRating(userId, pcId, rating);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("user-pc/{userId}")]
        public async Task<IActionResult> GetUserPc(int userId)
        {
            try
            {
                var result = await _computerService.GetUserPc(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("part-details")]
        public async Task<IActionResult> GetSystemPartDetails([FromBody] SavePCRequest request)
        {
            var result = await _computerService.GetPartDetails(request);
            return Ok(result);
        }

        [HttpGet("cpu/{id}")]
        public async Task<IActionResult> GetCpu(int id)
        {
            var result = await _computerService.GetCpuById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("gpu/{id}")]
        public async Task<IActionResult> GetGpu(int id)
        {
            var result = await _computerService.GetGpuById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("memory/{id}")]
        public async Task<IActionResult> GetMemory(int id)
        {
            var result = await _computerService.GetMemoryById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("storage/{id}")]
        public async Task<IActionResult> GetStorage(int id)
        {
            var result = await _computerService.GetStorageById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("motherboard/{id}")]
        public async Task<IActionResult> GetMotherboard(int id)
        {
            var result = await _computerService.GetMotherboardById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("powersupply/{id}")]
        public async Task<IActionResult> GetPowerSupply(int id)
        {
            var result = await _computerService.GetPowerSupplyById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("case/{id}")]
        public async Task<IActionResult> GetCase(int id)
        {
            var result = await _computerService.GetCaseById(id);
            return result != null ? Ok(result) : NotFound();
        }
    }
}