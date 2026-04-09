using System.Windows;

namespace LayoutSystemDemo.Logging;

public class LogEventArgs : RoutedEventArgs
{
    public LogMessage Entry { get; }
    public LogEventArgs(RoutedEvent routedEvent, LogMessage entry) : base(routedEvent)
    {
        Entry = entry;
    }
}
