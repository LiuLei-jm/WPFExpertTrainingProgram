namespace TestabilityAndDecoupling.Services;

public interface IDataService
{
    Task<string> LoadAsync(CancellationToken token);
}
