using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AlgorithmicSystems.Data;

internal sealed class BinanceSocketClient
{
    private int _id = 0;

    private const string WS_URL = "wss://stream.binance.com:9443";
    private const string UNSUBSCRIBE = "UNSUBSCRIBE";

    public async Task<TimeSpan> SubscribeAsync(string data, Action<string> handleResponse)
    {
        using var client = new ClientWebSocket();
        var normalizedData = data.ToLower();
        var uri = new Uri(WS_URL + $"/ws/{GetFullStreamName(normalizedData)}");

        await client.ConnectAsync(uri, CancellationToken.None);

        var buffer = new ArraySegment<byte>(new byte[256]);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (var i = 0; i < 1000; i++)
        {
            var result = await client.ReceiveAsync(buffer, CancellationToken.None);

            if (!result.EndOfMessage)
            {
                throw new NotImplementedException("Possibility of partial reading of messages is not implemented");
            }

            var responseData = Encoding.UTF8.GetString(buffer);
            handleResponse(responseData);
        }

        stopwatch.Stop();

        await UnsubscribeAsync(client, normalizedData);
        client.Abort();

        return stopwatch.Elapsed;
    }

    private async Task UnsubscribeAsync(ClientWebSocket client, string normalizedData)
    {
        var requestData = new BinanceRequestDto(GetNextId(), UNSUBSCRIBE, GetFullStreamName(normalizedData));
        var serializedRequest = JsonSerializer.Serialize(requestData);
        var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(serializedRequest));

        await client.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private int GetNextId() => _id++;

    private string GetFullStreamName(string data) => $"{data}@aggTrade";

    private sealed class BinanceRequestDto
    {
        internal BinanceRequestDto(int id, string method, string param)
        {
            Id = id;
            Method = method;
            Params = new List<string> { param };
        }

        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("method")]
        public string Method { get; init; }

        [JsonPropertyName("params")]
        public List<string> Params { get; init; }
    }
}
