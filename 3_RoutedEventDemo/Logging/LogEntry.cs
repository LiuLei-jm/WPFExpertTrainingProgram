namespace RoutedEventDemo.Logging;

public class LogEntry
{
    public DateTime Time { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public override string ToString()
    {
        return $"[{Time:HH:mm:ss:fff}] [{Level}] [{Message}]";
    }
}
