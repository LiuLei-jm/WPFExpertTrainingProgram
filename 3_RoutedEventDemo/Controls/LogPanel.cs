using RoutedEventDemo.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RoutedEventDemo.Controls;

public class LogPanel : ListBox
{
    private const int MaxLogs = 1000;
    public ObservableCollection<LogEntry> Logs = new ObservableCollection<LogEntry>();
    private ICollectionView _view;
    public LogLevel FilterLevel { get; set; } = LogLevel.Info;
    public string SearchText { get; set; } = string.Empty;
    private bool _isPaused = false;
    public LogPanel()
    {
        _view = CollectionViewSource.GetDefaultView(Logs);
        _view.Filter = Filter;
        ItemsSource = _view;
        this.Loaded += (s, e) =>
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.AddHandler(
                LogManager.LogEvent,
                new RoutedEventHandler(OnLogEvent),
                true);
        };
        this.Unloaded += (s, e) =>
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.RemoveHandler(
                LogManager.LogEvent,
                new RoutedEventHandler(OnLogEvent));
        };
    }

    private void OnLogEvent(object sender, RoutedEventArgs e)
    {
        if (e is LogEventArgs log)
        {
            if (log.Entry.Level >= FilterLevel)
            {
                Logs.Add(log.Entry);
                if (Logs.Count > MaxLogs) Logs.RemoveAt(0);
                if (!_isPaused) ScrollIntoView(log.Entry);
            }
        }
    }

    private bool Filter(object obj)
    {
        if (obj is not LogEntry log) return false;
        if (log.Level < FilterLevel) return false;
        if (!string.IsNullOrWhiteSpace(SearchText) &&
            !log.Message.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) return false;
        return true;
    }
    public void PauseScrolling()
    {
        _isPaused = true;
    }
    public void ResumeScrolling()
    {
        _isPaused = false;
        ScrollIntoView(Logs[Logs.Count - 1]);
    }
    public void SetFilterLevel(LogLevel level)
    {
        FilterLevel = level;
        _view.Refresh();
    }
    public void SetSearchText(string text)
    {
        SearchText = text;
        _view.Refresh();
    }
}
