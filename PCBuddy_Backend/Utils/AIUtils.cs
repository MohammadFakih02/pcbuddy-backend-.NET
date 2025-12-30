using FuzzySharp;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace PCBuddy_Backend.Utils
{
    public static class AIUtils
    {
        public static T? ExtractAndParseJson<T>(string text)
        {
            try
            {
                var match = Regex.Match(text, @"\{.*\}", RegexOptions.Singleline);

                if (!match.Success)
                    return default;

                string jsonString = match.Value;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                return JsonSerializer.Deserialize<T>(jsonString, options);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parse Error: {ex.Message}");
                return default;
            }
        }
        public static T? FuzzyMatch<T>(IEnumerable<T> items, string searchTerm, Func<T, string> nameSelector, int threshold = 50)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || items == null || !items.Any())
                return default;

            var result = Process.ExtractOne(
                searchTerm,
                items.Select(nameSelector),
                scorer: null
            );

            if (result != null && result.Score >= threshold)
            {
                return items.FirstOrDefault(i => nameSelector(i) == result.Value);
            }

            return default;
        }

        public static string? FormatImageUrl(string? url)
        {
            if (!string.IsNullOrEmpty(url) && url.StartsWith("//"))
            {
                return $"https:{url}";
            }
            return url;
        }
    }
}