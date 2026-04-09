using LayoutSystemDemo.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LayoutSystemDemo.Controls;

public class LogPanel : ContentControl
{
    private const int MaxLogs = 1000;
    public ObservableCollection<LogMessage> Logs = new ObservableCollection<LogMessage>();
    private ICollectionView _view;
    public LogLevel FilterLevel { get; set; } = LogLevel.Info;
    public string SearchText { get; set; } = string.Empty;
    private object _logsLock = new object();
    private ListBox? _listBox;
    private ScrollViewer? _scrollViewer;
    private bool _isPaused = false;
    public LogPanel()
    {
        BindingOperations.EnableCollectionSynchronization(Logs, _logsLock);
        _view = CollectionViewSource.GetDefaultView(Logs);
        _view.Filter = Filter;

        _listBox = new ListBox
        {
            ItemsSource = _view,
            ItemTemplate = (DataTemplate)Application.Current.FindResource("LogMessageDataTemplate"),
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
        };
        this.Content = _listBox;
        this.Loaded += OnLoaded;
        this.Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Window parentWindow = Window.GetWindow(this);
        parentWindow?.RemoveHandler(LogManager.LogEvent, new RoutedEventHandler(OnLogEvent));
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _scrollViewer = GetScrollViewer(_listBox);
        Window parentWindow = Window.GetWindow(this);
        parentWindow?.AddHandler(LogManager.LogEvent, new RoutedEventHandler(OnLogEvent), true);
    }

    private void OnLogEvent(object sender, RoutedEventArgs e)
    {
        if (e.Handled) return;
        if (e is LogEventArgs logEventArgs)
        {
            lock (_logsLock)
            {
                Logs.Add(logEventArgs.Entry);
                if (Logs.Count > MaxLogs) Logs.RemoveAt(0);
            }
            if (!_isPaused) ScrollToBottom();
        }
        e.Handled = true;
    }

    private void ScrollToBottom()
    {
        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
        {
            _scrollViewer?.ScrollToEnd();
        }));
    }

    private ScrollViewer? GetScrollViewer(DependencyObject obj)
    {
        if (obj is ScrollViewer sv) return sv;
        for (int i = 0; VisualTreeHelper.GetChildrenCount(obj) > 0; i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);
            var result = GetScrollViewer(child);
            if (result != null) return result;
        }
        return null;
    }

    private bool Filter(object obj)
    {
        if (obj is not LogMessage log) return false;
        if (log.Level < FilterLevel) return false;
        if (!string.IsNullOrWhiteSpace(SearchText) && !log.Message.Contains(SearchText)) return false;
        return true;
    }
    public void PauseScrolling()
    {
        _isPaused = true;
    }
    public void ResumeScrolling()
    {
        _isPaused = false;
        if (Logs.Count > 0) ScrollToBottom();
    }
    public void SetFilterLevel(LogLevel level)
    {
        if (FilterLevel != level)
        {
            FilterLevel = level;
            _view.Refresh();
            if (!_isPaused) ScrollToBottom();
        }
    }
    public void SetSearchText(string text)
    {
        if (SearchText != text)
        {
            SearchText = text;
            _view.Refresh();
            if (!_isPaused) ScrollToBottom();
        }
    }
}
