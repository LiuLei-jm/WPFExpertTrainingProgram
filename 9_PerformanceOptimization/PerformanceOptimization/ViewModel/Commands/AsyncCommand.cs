using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PerformanceOptimization.ViewModel.Commands;

public class AsyncCommand : ObservableObject, ICommand
{
    private readonly Func<CancellationToken, Task> _executeAsync;
    private readonly Func<bool>? _canExecute;
    private CancellationTokenSource _cts;
    private bool _isExecuting;
    public bool IsExecuting
    {
        get => _isExecuting;
        set
        {
            if (SetProperty(ref _isExecuting, value)) RaiseCanExecuteChanged();
        }
    }
    public ICommand CancelCommand { get; }
    public AsyncCommand(Func<CancellationToken, Task> executeAsync, Func<bool>? canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
        CancelCommand = new RelayCommand(
            Cancel,
            () => IsExecuting
            );
    }

    private void Cancel()
    {
        if (_cts != null && !_cts.IsCancellationRequested) _cts.Cancel();
    }

    private void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return !IsExecuting && (_canExecute?.Invoke() ?? true);
    }

    public async void Execute(object? parameter)
    {
        await ExecuteAsync();
    }

    private async Task ExecuteAsync()
    {
        if (IsExecuting) return ;
        IsExecuting = true;
        _cts = new CancellationTokenSource();
        try
        {
            await _executeAsync(_cts.Token);
        }
        catch(OperationCanceledException)
        {
            Debug.WriteLine("用户已取消任务");
        }
        catch(Exception ex)
        {
            Debug.WriteLine($"发送错误：{ex.Message}");
        }
        finally
        {
            IsExecuting = false;
            _cts.Dispose();
            _cts = null;
        }
    }
}
