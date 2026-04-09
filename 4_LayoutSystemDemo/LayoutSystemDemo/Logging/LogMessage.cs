namespace LayoutSystemDemo.Logging;

public class LogMessage
{
    public DateTime Time { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public override string ToString() => $"{Time:HH:mm:ss.fff} [{Level}] {Message}";
}
