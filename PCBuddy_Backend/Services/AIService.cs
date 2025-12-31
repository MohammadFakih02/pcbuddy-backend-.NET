using System.Text;
using System.Text.Json;
using PCBuddy_Backend.DTOs;
using PCBuddy_Backend.DTOs.AI;
using PCBuddy_Backend.Models;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ComputerService _computerService;
        private const string Model = "gemini-2.0-flash-lite";

        public AIService(IConfiguration config, HttpClient httpClient, ComputerService computerService)
        {
            _apiKey = config["GEMINI_API_KEY"]
                      ?? config["Gemini:ApiKey"]
                      ?? throw new ArgumentNullException("Gemini API Key is missing. Please add GEMINI_API_KEY to your .env file.");

            _httpClient = httpClient;
            _computerService = computerService;
        }

        private async Task<string?> GetGeminiResponseAsync(string prompt)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, jsonContent);

                // Better error logging
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Gemini API Error ({response.StatusCode}): {errorContent}");
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseString);

                if (doc.RootElement.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    return candidates[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();
                }

                Console.WriteLine("Gemini returned no candidates.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gemini API Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<LaptopAssessmentResponse?> AssessLaptop(string laptopName, string? details)
        {
            var prompt = $@"
            Given the following laptop name and optional details, provide an assessment including specs, ratings, expected FPS, and thermal performance.
            
            Laptop Name: ""{laptopName}""
            Details: ""{details ?? "None"}""

            Return a valid JSON object with this structure:
            {{
                ""laptopName"": ""string"",
                ""specs"": {{ ""cpu"": """", ""gpu"": """", ""ram"": """", ""storage"": """", ""display"": """" }},
                ""ratings"": {{ ""overall"": ""0-10"", ""cpu"": ""0-10"", ""gpu"": ""0-10"" }},
                ""fps"": {{
                    ""Cyberpunk 2077"": {{ ""low"": 0, ""medium"": 0, ""high"": 0, ""ultra"": 0 }},
                    ""Red Dead Redemption 2"": {{ ""low"": 0, ""medium"": 0, ""high"": 0, ""ultra"": 0 }},
                    ""Counter Strike 2"": {{ ""low"": 0, ""medium"": 0, ""high"": 0, ""ultra"": 0 }},
                    ""Fortnite"": {{ ""low"": 0, ""medium"": 0, ""high"": 0, ""ultra"": 0 }},
                    ""Call of Duty: Warzone"": {{ ""low"": 0, ""medium"": 0, ""high"": 0, ""ultra"": 0 }}
                }},
                ""thermalPerformance"": ""string"",
                ""summary"": ""string""
            }}";

            var rawResponse = await GetGeminiResponseAsync(prompt);
            return rawResponse != null ? AIUtils.ExtractAndParseJson<LaptopAssessmentResponse>(rawResponse) : null;
        }

        public async Task<SystemPartsResponse?> GetPCRecommendation(string userPrompt)
        {
            var motherboards = await _computerService.GetMotherboards();
            var motherboardNames = string.Join(", ", motherboards.Select(m => m.Name));

            var prompt = $@"
            Suggest a PC build based on this user prompt: ""{userPrompt}""
            
            Constraint: ONLY suggest motherboards from this list: [{motherboardNames}]
            
            Return JSON only:
            {{
              ""cpu"": ""Exact Model Name"",
              ""gpu"": ""Exact Model Name"",
              ""ram"": ""Model Name"",
              ""psu"": ""Model Name"",
              ""case"": ""Model Name"",
              ""hdd"": ""Model Name"",
              ""ssd"": ""Model Name"",
              ""motherboard"": ""Exact Model Name from list""
            }}";

            var rawResponse = await GetGeminiResponseAsync(prompt);
            if (rawResponse == null) return null;

            var aiSuggestion = AIUtils.ExtractAndParseJson<PCRecommendationRawResponse>(rawResponse);
            if (aiSuggestion == null) return null;

            var cpus = await _computerService.GetCPUs();
            var gpus = await _computerService.GetGPUs();
            var rams = await _computerService.GetMemory();
            var psus = await _computerService.GetPowerSupplies();
            var cases = await _computerService.GetCases();
            var storages = await _computerService.GetStorage();

            var matchedCpu = AIUtils.FuzzyMatch(cpus, aiSuggestion.Cpu, x => x.Name);
            var matchedGpu = AIUtils.FuzzyMatch(gpus, aiSuggestion.Gpu, x => x.Name);
            var matchedRam = AIUtils.FuzzyMatch(rams, aiSuggestion.Ram, x => x.Name);
            var matchedPsu = AIUtils.FuzzyMatch(psus, aiSuggestion.Psu, x => x.Name);
            var matchedCase = AIUtils.FuzzyMatch(cases, aiSuggestion.Case, x => x.Name);
            var matchedMobo = AIUtils.FuzzyMatch(motherboards, aiSuggestion.Motherboard, x => x.Name);

            var matchedHdd = AIUtils.FuzzyMatch(storages, aiSuggestion.Hdd, x => x.Name);
            var matchedSsd = AIUtils.FuzzyMatch(storages, aiSuggestion.Ssd, x => x.Name);

            var detailsRequest = new SavePCRequest(
                matchedCpu?.Id, matchedGpu?.Id, matchedRam?.Id,
                matchedHdd?.Id, matchedSsd?.Id, matchedMobo?.Id,
                matchedPsu?.Id, matchedCase?.Id
            );

            return await _computerService.GetPartDetails(detailsRequest);
        }

        public async Task<FpsEstimates?> GetPerformance(string cpu, string gpu, string ram, string gameName)
        {
            var prompt = $@"
            Estimate FPS (Low, Medium, High, Ultra) at 1080p for:
            Game: {gameName}
            Hardware: CPU {cpu}, GPU {gpu}, RAM {ram}.
            
            Return JSON: {{ ""low"": int, ""medium"": int, ""high"": int, ""ultra"": int }}";

            var rawResponse = await GetGeminiResponseAsync(prompt);
            return rawResponse != null ? AIUtils.ExtractAndParseJson<FpsEstimates>(rawResponse) : null;
        }

        public async Task<Dictionary<string, FpsEstimates>?> GetTemplateGraph(string cpu, string gpu, string ram, List<string>? games)
        {
            var gameList = games != null && games.Any()
                ? string.Join(", ", games)
                : "Cyberpunk 2077, Red Dead Redemption 2, Counter Strike 2, Fortnite, Call of Duty: Warzone";

            var prompt = $@"
            Estimate average FPS (Low, Medium, High, Ultra) at 1080p for these games: [{gameList}]
            Hardware: CPU {cpu}, GPU {gpu}, RAM {ram}.

            Return a JSON object where keys are game names and values are objects with low/medium/high/ultra integers.";

            var rawResponse = await GetGeminiResponseAsync(prompt);
            return rawResponse != null ? AIUtils.ExtractAndParseJson<Dictionary<string, FpsEstimates>>(rawResponse) : null;
        }

        public async Task<CompatibilityResponse?> CheckCompatibility(FullSystemRequest system)
        {
            var motherboards = await _computerService.GetMotherboards();
            var moboNames = string.Join(", ", motherboards.Select(m => m.Name));

            var prompt = $@"
            Check compatibility for this PC:
            CPU: {system.Cpu}, GPU: {system.Gpu}, RAM: {system.Ram}, 
            Mobo: {system.Motherboard}, PSU: {system.Psu}, Case: {system.Case}, 
            Storage: {system.Storage}, SSD: {system.Ssd}, HDD: {system.Hdd}.

            Available Motherboards to suggest replacements: [{moboNames}]

            Return JSON:
            {{
              ""compatibilityIssues"": [
                {{
                  ""issue"": ""Description"",
                  ""causingParts"": [""Part Name""],
                  ""suggestedParts"": [ {{ ""name"": ""Part Name"", ""type"": ""CPU/GPU/etc"" }} ]
                }}
              ]
            }}";

            var rawResponse = await GetGeminiResponseAsync(prompt);
            return rawResponse != null ? AIUtils.ExtractAndParseJson<CompatibilityResponse>(rawResponse) : null;
        }

        public async Task<PCRatingResponse?> RatePC(FullSystemRequest system)
        {
            var prompt = $@"
            Rate this PC build on a scale of 1-10 for CPU, GPU, and Overall.
            Components: {system.Cpu}, {system.Gpu}, {system.Ram}, {system.Motherboard}, {system.Psu}.
            
            Return JSON: {{ ""cpu"": number, ""gpu"": number, ""overall"": number }}";

            var rawResponse = await GetGeminiResponseAsync(prompt);
            return rawResponse != null ? AIUtils.ExtractAndParseJson<PCRatingResponse>(rawResponse) : null;
        }
    }
}