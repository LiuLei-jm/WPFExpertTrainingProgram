namespace TestabilityAndDecoupling.Services.Interfaces;

public interface IDataService
{
    Task<string> LoadAsync(CancellationToken token);
}
