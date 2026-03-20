using System.Windows;

namespace RoutedEventDemo.Logging;

public static class LogManager
{
    public static readonly RoutedEvent LogEvent
        = EventManager.RegisterRoutedEvent(
            "Log",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(LogManager)
            );

    public static void Log(LogLevel level, string message)
    {
        var entry = new LogEntry
        {
            Time = DateTime.Now,
            Level = level,
            Message = message
        };
        Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
        {
            var args = new LogEventArgs(LogEvent, entry);
            Application.Current.MainWindow?.RaiseEvent(args);
        }));
    }
}
