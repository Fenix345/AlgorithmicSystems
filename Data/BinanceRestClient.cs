using AlgorithmicSystems.Exceptions;

namespace AlgorithmicSystems.Data;

internal sealed class BinanceRestClient : IDisposable
{
    private readonly HttpClient _httpClient;

    private const string API_URL = "https://api.binance.com/api/v3/";

    internal BinanceRestClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(API_URL);
    }

    public async Task<string> GetInfoAsync(string data)
    {
        var normalizedData = data.ToUpper();
        var response = await _httpClient.GetAsync($"exchangeInfo?symbol={normalizedData}");

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new BinanceClientException(content);
        }

        return await response.Content.ReadAsStringAsync();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
