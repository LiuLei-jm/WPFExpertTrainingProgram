using AdvancedMVVMNotCRUD.Commands;

namespace AdvancedMVVMNotCRUD.ViewModels;

public class DownloadViewModel : ObservableObject
{
    private string _resultText;
    public string ResultText
    {
        get => _resultText;
        set => SetProperty(ref _resultText, value);
    }
    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    public AsyncCommand DownloadCommand { get; }
    public RelayCommand ClearCommand { get; }
    public DownloadViewModel()
    {
        DownloadCommand = new AsyncCommand(ExecuteDownloadAsync, CanDownload);
        ClearCommand = new RelayCommand(_ => { ResultText = ""; ErrorMessage = ""; });
        SimpleMessenger.Subscribe<DataDownloadedMessage>(msg =>
        {
            Console.WriteLine($"收到内部消息：{msg.data}");
        });
    }

    private bool CanDownload()
    {
        return string.IsNullOrEmpty(ErrorMessage);
    }

    private async Task ExecuteDownloadAsync(CancellationToken token)
    {
        ErrorMessage = string.Empty;
        ResultText = "请求发送中...";
        try
        {
            await Task.Delay(3000, token);
            ResultText = $"下载成功：数据ID：{Guid.NewGuid()}";
            SimpleMessenger.Send(new DataDownloadedMessage(ResultText));
        }
        catch (OperationCanceledException)
        {
            ResultText = "用户已取消下载";
            throw;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"下载失败：{ex.Message}";
            ResultText = "";
            throw;
        }
    }
}
