using Microsoft.AspNetCore.Mvc;
using PCBuddy_Backend.DTOs.AI;
using PCBuddy_Backend.Services;

namespace PCBuddy_Backend.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        private readonly AIService _aiService;

        public AIController(AIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("assess-laptop")]
        public async Task<IActionResult> AssessLaptop([FromBody] AssessLaptopRequest request)
        {
            var result = await _aiService.AssessLaptop(request.LaptopName, request.Details);
            if (result == null) return BadRequest("Failed to generate assessment.");
            return Ok(new { success = true, response = result });
        }

        [HttpPost("generate-pc")]
        public async Task<IActionResult> GetPC([FromBody] PCPromptRequest request)
        {
            var result = await _aiService.GetPCRecommendation(request.Prompt);
            if (result == null) return BadRequest("Failed to generate PC recommendation.");
            return Ok(new { success = true, response = result });
        }

        [HttpPost("performance")]
        public async Task<IActionResult> GetPerformance([FromBody] PerformanceRequest request)
        {
            var result = await _aiService.GetPerformance(request.Cpu, request.Gpu, request.Ram, request.GameName);
            if (result == null) return BadRequest("Failed to estimate performance.");
            return Ok(new { success = true, response = result });
        }

        [HttpPost("template-graph")]
        public async Task<IActionResult> GetTemplateGraph([FromBody] TemplateGraphRequest request)
        {
            var result = await _aiService.GetTemplateGraph(request.Cpu, request.Gpu, request.Ram, request.Applications);
            if (result == null) return BadRequest("Failed to generate graph data.");
            return Ok(new { success = true, response = result });
        }

        [HttpPost("check-compatibility")]
        public async Task<IActionResult> CheckCompatibility([FromBody] FullSystemRequest request)
        {
            var result = await _aiService.CheckCompatibility(request);
            if (result == null) return BadRequest("Failed to check compatibility.");
            return Ok(new { success = true, response = result });
        }

        [HttpPost("rate-pc")]
        public async Task<IActionResult> RatePC([FromBody] FullSystemRequest request)
        {
            var result = await _aiService.RatePC(request);
            if (result == null) return BadRequest("Failed to rate PC.");
            return Ok(new { success = true, response = result });
        }
    }
}