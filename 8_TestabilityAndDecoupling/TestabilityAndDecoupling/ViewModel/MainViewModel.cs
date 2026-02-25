using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestabilityAndDecoupling.Services;

namespace TestabilityAndDecoupling.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly IDataService _dataService;
    private readonly IUserNotification _notification;
    private CancellationTokenSource? _cts;
    public MainViewModel(IDataService dataService, IUserNotification notification)
    {
        _dataService = dataService;
        _notification = notification;
    }
    [ObservableProperty]
    private string _result = string.Empty;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
    private bool _isLoading;
    private bool CanLoad() => !IsLoading;
    [RelayCommand(CanExecute = nameof(CanLoad))]
    private async Task LoadAsync()
    {
        _cts = new CancellationTokenSource();
        IsLoading = true;
        try
        {
            Result = await _dataService.LoadAsync(_cts.Token);
            _notification.ShowMessage("Data loaded successfully!");
        }
        catch (OperationCanceledException)
        {
            _notification.ShowMessage("Data loading canceled.");
        }
        catch (Exception ex)
        {
            _notification.ShowMessage($"Error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
    [RelayCommand(CanExecute = nameof(IsLoading))]
    private void Cancel()
    {
        _cts?.Cancel();
    }
}
