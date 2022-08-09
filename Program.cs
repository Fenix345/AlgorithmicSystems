using AlgorithmicSystems.Data;
using AlgorithmicSystems.Enums;
using AlgorithmicSystems.Exceptions;

using var binanceClient = new BinanceClient();

while (true)
{
    try
    {
        var inputString = Console.ReadLine();
        var command = CommandParser.Parse(inputString!);

        switch(command.Type)
        {
            case CommandType.Info:
                var content = await binanceClient.GetInfoAsync(command.Value);
                Console.WriteLine(content);
                break;

            case CommandType.Subscribe:
                var time = await binanceClient.SubscribeAsync(command.Value, (string data) => Console.WriteLine(data));
                Console.WriteLine($"Time spent reading 1000 packets: {time}");
                break;

            default:
                throw new NotImplementedException();
        }
    }
    catch (BinanceClientException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (CommandParserException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("oops something went wrong:");
        Console.WriteLine(ex);
    }
}