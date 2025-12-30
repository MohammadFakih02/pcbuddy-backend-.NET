using System.Text.Json.Serialization;

namespace PCBuddy_Backend.DTOs.AI
{
    public class AIPartSuggestion
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public class FpsEstimates
    {
        [JsonPropertyName("low")]
        public int Low { get; set; }

        [JsonPropertyName("medium")]
        public int Medium { get; set; }

        [JsonPropertyName("high")]
        public int High { get; set; }

        [JsonPropertyName("ultra")]
        public int Ultra { get; set; }
    }

    public class LaptopAssessmentResponse
    {
        public string LaptopName { get; set; } = string.Empty;
        public LaptopSpecs Specs { get; set; } = new();
        public LaptopRatings Ratings { get; set; } = new();

        public Dictionary<string, FpsEstimates> Fps { get; set; } = new();

        public string ThermalPerformance { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }

    public class LaptopSpecs
    {
        public string Cpu { get; set; } = string.Empty;
        public string Gpu { get; set; } = string.Empty;
        public string Ram { get; set; } = string.Empty;
        public string Storage { get; set; } = string.Empty;
        public string Display { get; set; } = string.Empty;
    }

    public class LaptopRatings
    {
        public string Overall { get; set; } = "0";
        public string Cpu { get; set; } = "0";
        public string Gpu { get; set; } = "0";
    }

    public class PCRecommendationRawResponse
    {
        public string Cpu { get; set; } = string.Empty;
        public string Gpu { get; set; } = string.Empty;
        public string Ram { get; set; } = string.Empty;
        public string Psu { get; set; } = string.Empty;
        public string Case { get; set; } = string.Empty;

        [JsonPropertyName("hdd")]
        public string Hdd { get; set; } = string.Empty;

        [JsonPropertyName("ssd")]
        public string Ssd { get; set; } = string.Empty;

        public string Motherboard { get; set; } = string.Empty;
    }

    public class CompatibilityResponse
    {
        public List<CompatibilityIssue> CompatibilityIssues { get; set; } = new();
    }

    public class CompatibilityIssue
    {
        public string Issue { get; set; } = string.Empty;
        public List<string> CausingParts { get; set; } = new();
        public List<AIPartSuggestion> SuggestedParts { get; set; } = new();
    }

    public class PCRatingResponse
    {
        [JsonPropertyName("cpu")]
        public double CpuRating { get; set; }

        [JsonPropertyName("gpu")]
        public double GpuRating { get; set; }

        [JsonPropertyName("overall")]
        public double OverallRating { get; set; }
    }
}