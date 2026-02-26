using PerformanceOptimization.Models;
using PerformanceOptimization.Utilities.Collections;
using PerformanceOptimization.ViewModel.Commands;
using System.Collections.Concurrent;
using System.Windows.Threading;

namespace PerformanceOptimization.ViewModel;

public class MainViewModel
{
    private const int MaxLogCount = 1_000_000;
    private const int FlushBatchSize = 1000;

    public BulkObservableCollection<LogItem> Logs { get; } = new BulkObservableCollection<LogItem>();
    private readonly ConcurrentQueue<LogItem> _buffer = new ConcurrentQueue<LogItem>();

    private DispatcherTimer _uiTimer;
    public RelayCommand StartCommand { get; }
    public RelayCommand StopCommand { get; }
    private CancellationTokenSource? _cts;
    public MainViewModel()
    {
        StartCommand = new RelayCommand(Start);
        StopCommand = new RelayCommand(Stop);

        _uiTimer = new DispatcherTimer(
            TimeSpan.FromMilliseconds(50),
            DispatcherPriority.Background,
            FlushToUI,
            Dispatcher.CurrentDispatcher);
    }

    private void Start()
    {
        _cts = new CancellationTokenSource();
        Task.Run(() =>
        {
            int i = 0;
            while (!_cts.IsCancellationRequested)
            {
                _buffer.Enqueue(new LogItem
                {
                    Time = DateTime.Now,
                    Message = $"Log message {i++}"
                });
                if (i % 100 == 0) Thread.Sleep(1);
            }
        });
        _uiTimer.Start();
    }

    private void Stop()
    {
        _cts?.Cancel();
        _uiTimer.Stop();
    }

    private void FlushToUI(object? sender, EventArgs e)
    {
        if (_buffer.IsEmpty) return;
        var list = new List<LogItem>();
        while (list.Count < FlushBatchSize && _buffer.TryDequeue(out var item))
        {
            list.Add(item);
        }
        if (list.Count > 0)
        {
            Logs.AddRange(list);
            while (Logs.Count > MaxLogCount)
            {
                Logs.RemoveAt(0);
            }
        }
    }
}
