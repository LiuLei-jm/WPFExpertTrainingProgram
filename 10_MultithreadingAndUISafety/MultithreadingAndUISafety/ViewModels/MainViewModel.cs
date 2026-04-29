using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MultithreadingAndUISafety.Models;
using System.Collections.ObjectModel;

namespace MultithreadingAndUISafety.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ObservableCollection<string> RealTimeLogs { get; } = [];
    [ObservableProperty]
    private int _progressValue;
    [ObservableProperty]
    private string _statusText = "准备就绪";
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
    private bool _isProcessing;

    private CancellationTokenSource _cts = null!;
    private bool CanStart() => !IsProcessing;

    [RelayCommand(CanExecute = nameof(CanStart))]
    private async Task StartAsync()
    {
        IsProcessing = true;
        ProgressValue = 0;
        StatusText = "任务正在初始化...";
        RealTimeLogs.Clear();
        _cts = new CancellationTokenSource();
        var progressReporter = new Progress<ProgressStatus>(status =>
        {
            ProgressValue = status.Percentage;
            StatusText = status.CurrentAction;
            RealTimeLogs.Add($"[{DateTime.Now:HH:mm:ss.fff}] {status.CurrentAction}");
        });
        try
        {
            await Task.Run(() => DoHeavyWord(_cts.Token, progressReporter));
            StatusText = "任务顺利完成";
        }
        catch (OperationCanceledException)
        {
            StatusText = "任务已被取消";
        }
        catch (Exception ex)
        {
            StatusText = $"发生错误：{ex.Message}";
        }
        finally
        {
            _cts.Dispose();
            _cts = null!;
            IsProcessing = false;
        }
    }

    private static void DoHeavyWord(CancellationToken token, IProgress<ProgressStatus> progressReporter)
    {
        int totalSteps = 100;
        for (int i = 0; i < totalSteps; i++)
        {
            token.ThrowIfCancellationRequested();
            Thread.Sleep(100);
            progressReporter?.Report(new ProgressStatus
            {
                Percentage = i + 1,
                CurrentAction = $"正在处理第 {i + 1} 条数据，共 {totalSteps} 条"
            });
        }
    }

    [RelayCommand(CanExecute = nameof(IsProcessing))]
    private void Cancel()
    {
        _cts?.Cancel();
        StatusText = "任务正在停止，请稍后...";
    }
}
