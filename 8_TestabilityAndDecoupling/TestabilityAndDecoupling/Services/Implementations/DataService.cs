using TestabilityAndDecoupling.Services.Interfaces;

namespace TestabilityAndDecoupling.Services;

public class DataService : IDataService
{
    public async Task<string> LoadAsync(CancellationToken token)
    {
        await Task.Delay(3000, token);
        return $"Current Time: {DateTime.Now}";
    }
}
