namespace PCBuddy_Backend.DTOs.AI
{
    public record AssessLaptopRequest(string LaptopName, string? Details);

    public record PCPromptRequest(string Prompt);

    public record PerformanceRequest(
        string Cpu,
        string Gpu,
        string Ram,
        string GameName
    );

    public record TemplateGraphRequest(
        string Cpu,
        string Gpu,
        string Ram,
        List<string>? Applications
    );

    public record FullSystemRequest(
        string Cpu,
        string Gpu,
        string Ram,
        string Storage,
        string Ssd,
        string Hdd,
        string Motherboard,
        string Psu,
        string Case
    );
}