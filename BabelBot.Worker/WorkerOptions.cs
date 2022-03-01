namespace BabelBot.Worker;

public class WorkerOptions
{
    public const string SectionKey = "Worker";

    public string[] Receivers { get; set; } = Array.Empty<string>();
    public string? Translator { get; set; }
}