using System.Windows;

namespace RoutedEventDemo.Logging;

public class LogEventArgs : RoutedEventArgs
{
    public LogEntry Entry { get; }
    public LogEventArgs(RoutedEvent routedEvent, LogEntry entry) : base(routedEvent)
    {
        Entry = entry;
    }
}
