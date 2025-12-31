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
        private const string Model = "gemini-2.5-flash-lite";

        public AIService(IConfiguration config, HttpClient httpClient, ComputerService computerService)
        {
            _apiKey = config["GEMINI_API_KEY"]
                      ?? config["Gemini:ApiKey"]
                      ?? throw new ArgumentNullException("Gemini API Key is missing.");

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
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Gemini API Error ({response.StatusCode}): {errorContent}");
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] Gemini Raw Response: {responseString}");
                using var doc = JsonDocument.Parse(responseString);

                if (doc.RootElement.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    return candidates[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gemini API Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<SystemPartsResponse?> GetPCRecommendation(string userPrompt)
        {
            var motherboards = await _computerService.GetMotherboards();
            var moboNames = string.Join(", ", motherboards.Take(50).Select(m => m.Name));

            var prompt = $@"
            Given the following user prompt, return a JSON object with the best PC components that match the user's needs.
            
            User Prompt: ""{userPrompt}""

            The AI should:
            1. Stay within the budget (if mentioned).
            2. Suggest specific, real-world component model names (e.g., 'AMD Ryzen 5 5600X', 'MSI GeForce RTX 3060 Ventus 2X').
            3. For the Motherboard, try to pick one from this list if suitable: [{moboNames}]. If none fit, suggest a compatible one.

            Return the response in the following JSON format:
            {{
              ""cpu"": ""specific model name"",
              ""gpu"": ""specific model name"",
              ""ram"": ""specific model name"",
              ""psu"": ""specific model name"",
              ""case"": ""specific model name"",
              ""hdd"": ""specific model name (or N/A)"",
              ""ssd"": ""specific model name (or N/A)"",
              ""motherboard"": ""specific model name""
            }}
            ";

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

            T? MatchPart<T>(IEnumerable<T> list, string? name, Func<T, string> selector)
            {
                if (string.IsNullOrWhiteSpace(name) || name.ToUpper() == "N/A" || name.ToUpper() == "NONE")
                    return default;

                return AIUtils.FuzzyMatch(list, name, selector, threshold: 60);
            }


            var matchedCpu = MatchPart(cpus, aiSuggestion.Cpu, x => x.Name);

            var matchedGpu = MatchPart(gpus, aiSuggestion.Gpu, x => $"{x.Name} {x.Chipset}");

            var matchedRam = MatchPart(rams, aiSuggestion.Ram, x => x.Name);
            var matchedPsu = MatchPart(psus, aiSuggestion.Psu, x => x.Name);
            var matchedCase = MatchPart(cases, aiSuggestion.Case, x => x.Name);
            var matchedMobo = MatchPart(motherboards, aiSuggestion.Motherboard, x => x.Name);

            var matchedHdd = MatchPart(storages, aiSuggestion.Hdd, x => $"{x.Name} {x.Capacity}GB");
            var matchedSsd = MatchPart(storages, aiSuggestion.Ssd, x => $"{x.Name} {x.Capacity}GB");

            var detailsRequest = new SavePCRequest(
                matchedCpu?.Id, matchedGpu?.Id, matchedRam?.Id,
                matchedHdd?.Id, matchedSsd?.Id, matchedMobo?.Id,
                matchedPsu?.Id, matchedCase?.Id
            );

            return await _computerService.GetPartDetails(detailsRequest);
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
            // We fetch motherboards just to provide suggestions, not to restrict checking
            var motherboards = await _computerService.GetMotherboards();
            var moboNames = string.Join(", ", motherboards.Take(50).Select(m => m.Name));

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