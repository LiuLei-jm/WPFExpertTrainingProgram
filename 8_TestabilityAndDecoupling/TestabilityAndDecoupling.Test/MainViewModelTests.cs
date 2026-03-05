using Moq;
using System.Threading.Tasks;
using TestabilityAndDecoupling.Services.Interfaces;
using TestabilityAndDecoupling.ViewModel;

namespace TestabilityAndDecoupling.Test;

public class MainViewModelTests
{
    private readonly MainViewModel _stu;
    private readonly Mock<IDataService> _dataServiceMock = new Mock<IDataService>();
    private readonly Mock<IUserNotification> _notificationMock = new Mock<IUserNotification>();
    public MainViewModelTests()
    {
        _stu = new MainViewModel(_dataServiceMock.Object, _notificationMock.Object);
    }
    [Fact]
    public async Task Load_Successs_ShouldUpdateResult()
    {
        _dataServiceMock.Setup(x => x.LoadAsync(It.IsAny<CancellationToken>())).ReturnsAsync("Test");
        await _stu.LoadCommand.ExecuteAsync(null);
        Assert.Equal("Test", _stu.Result);
        _notificationMock.Verify(x => x.ShowMessage("Data loaded successfully!"), Times.Once);
    }
    [Fact]
    public async Task Load_ShouldDisableWhileRunning()
    {
        var tcs = new TaskCompletionSource<string>();
        _dataServiceMock.Setup(x => x.LoadAsync(It.IsAny<CancellationToken>())).Returns(tcs.Task);
        var task = _stu.LoadCommand.ExecuteAsync(null);
        Assert.False(_stu.LoadCommand.CanExecute(null));
        tcs.SetResult("OK");
        await task;
        Assert.True(_stu.LoadCommand.CanExecute(null));
    }
    [Fact]
    public async Task Cancel_ShouldCallCancelToken()
    {
        _dataServiceMock.Setup(x => x.LoadAsync(It.IsAny<CancellationToken>())).Returns(async (CancellationToken token) =>
        {
            await Task.Delay(3000, token);
            return "";
        });
        var task = _stu.LoadCommand.ExecuteAsync(null);
        _stu.CancelCommand.Execute(null);
        await task;
        _notificationMock.Verify(x => x.ShowMessage("Data loading canceled."), Times.Once);
    }
}