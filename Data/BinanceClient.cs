namespace AlgorithmicSystems.Data;

internal sealed class BinanceClient : IDisposable
{
    private readonly BinanceRestClient _httpClient;
    private readonly BinanceSocketClient _socketClient;

    internal BinanceClient()
    {
        _httpClient = new BinanceRestClient();
        _socketClient = new BinanceSocketClient();
    }

    public async Task<string> GetInfoAsync(string data)
    {
        return await _httpClient.GetInfoAsync(data);
    }

    public async Task<TimeSpan> SubscribeAsync(string data, Action<string> handleResponse)
    {
        return await _socketClient.SubscribeAsync(data, handleResponse);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
