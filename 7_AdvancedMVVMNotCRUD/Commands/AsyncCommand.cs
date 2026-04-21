using AdvancedMVVMNotCRUD.ViewModels;
using System.Windows.Input;

namespace AdvancedMVVMNotCRUD.Commands;

public class AsyncCommand : ObservableObject, ICommand
{
    private readonly Func<CancellationToken, Task> _executeAsync;
    private readonly Func<bool> _canExecute;
    private CancellationTokenSource _cts;
    private bool _isExecuting;
    public bool IsExecuting
    {
        get => _isExecuting;
        private set
        {
            if (SetProperty(ref _isExecuting, value))
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
    public ICommand CancelCommand { get; }

    public AsyncCommand(Func<CancellationToken, Task> executeAsync, Func<bool> canExecute = null!)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
        CancelCommand = new RelayCommand(_ => Cancel(), _ => IsExecuting);
    }

    private void Cancel()
    {
        if (_cts != null && !_cts.IsCancellationRequested)
            _cts.Cancel();
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
    {
        return !IsExecuting && (_canExecute == null || _canExecute());
    }

    public async void Execute(object? parameter)
    {
        if (IsExecuting)
            return;
        _cts = new CancellationTokenSource();
        IsExecuting = true;
        try
        {
            await _executeAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("任务已取消");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生错误：{ex.Message}");
        }
        finally
        {
            IsExecuting = false;
            _cts.Dispose();
            _cts = null!;
        }
    }
}
