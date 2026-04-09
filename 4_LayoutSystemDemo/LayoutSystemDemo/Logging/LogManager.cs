using System.Windows;

namespace LayoutSystemDemo.Logging;

public static class LogManager
{
    public readonly static RoutedEvent LogEvent
        = EventManager.RegisterRoutedEvent(
            "Log",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(LogManager)
            );

    public static void Log(LogLevel level, string message)
    {
        var log = new LogMessage
        {
            Time = DateTime.Now,
            Level = level,
            Message = message
        };
        var args = new LogEventArgs(LogEvent, log);
        Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
        {
            Application.Current.MainWindow?.RaiseEvent(args);
        }));
    }

    public static void LogInfo(string message) => Log(LogLevel.Info, message);
    public static void LogWarning(string message) => Log(LogLevel.Warning, message);
    public static void LogError(string message) => Log(LogLevel.Error, message);
    public static void LogDebug(string message) => Log(LogLevel.Debug, message);
}
