using PerformanceOptimization.Models;
using PerformanceOptimization.Utilities.Collections;
using PerformanceOptimization.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PerformanceOptimization.ViewModel;

public class PerformanceViewModel : ObservableObject
{
    private const int MaxLogCount = 1_000_000;
    public ObservableCollection<LogItem> Logs { get; } = [];
    public BulkObservableCollection<LogItem> GoodLogs { get; } = [];
    private string _statusText = "准备就绪";
    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }
    public AsyncCommand LoadBadCommand { get; }
    public AsyncCommand LoadGoodCommand { get; }
    public RelayCommand ClearCommand { get; }
    public PerformanceViewModel()
    {
        LoadBadCommand = new AsyncCommand(ExecuteBadLoad);
        LoadGoodCommand = new AsyncCommand(ExecuteGoodLoad);
        ClearCommand = new RelayCommand(
            () =>
            {
                Logs.Clear();
                GoodLogs.Clear();
                StatusText = "准备就绪";
            }
            );
    }

    private async Task ExecuteGoodLoad(CancellationToken token)
    {
        GoodLogs.Clear();
        StatusText = "正在平滑加载中...(可以拖动窗口)";
        Stopwatch sw = Stopwatch.StartNew();
        await Task.Run(async () => {
            for(int batch = 0; batch<100; batch++)
            {
                var tempList = new LogItem[1000];
                for(int i = 0; i<1000; i++)
                {
                    int index = batch * 1000 + i;
                    tempList[i] = new LogItem
                    {
                        Id = index,
                        Message = $"【优化的数据】这是第 {index} 条日志",
                        BackgroundBrush = LogItem.GetOptimizedBrush()
                    };
                }
                await Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        GoodLogs.AddRange(tempList);
                    },System.Windows.Threading.DispatcherPriority.Background
                    );
            }
        },token);
        sw.Stop();
        StatusText = $"平滑加载完成，总耗时：{sw.ElapsedMilliseconds} ms"; 
    }

    private async Task ExecuteBadLoad(CancellationToken token)
    {
        Logs.Clear();
        StatusText = "正在暴力加载中...(无法拖动窗口)";
        Stopwatch sw = Stopwatch.StartNew();
        for(int i = 0; i< 100000; i++)
        {
            Logs.Add(new LogItem
            {
                Id = i,
                Message = $"【糟糕的数据】这是第 {i} 条日志",
                BackgroundBrush = LogItem.GetBadBrush()
            });
        }
        sw.Stop();
        StatusText = $"保留加载完成，总耗时：{sw.ElapsedMilliseconds} ms";
    }
}
