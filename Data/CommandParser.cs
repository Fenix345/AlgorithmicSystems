using AlgorithmicSystems.Enums;
using AlgorithmicSystems.Exceptions;

namespace AlgorithmicSystems.Data;

internal static class CommandParser
{
    private const string SPLITTER = " ";

    internal static Command Parse(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            throw new CommandParserException("Data is null or empty");
        }

        var dataItems = data
            .Trim()
            .Split(SPLITTER, StringSplitOptions.RemoveEmptyEntries); 

        if (dataItems.Count() != 2)
        {
            throw new CommandParserException("The command must have the following format: <command> <value>");
        }

        var command = dataItems.First();
        var commandValue = dataItems.Last();

        if (!Enum.TryParse(command, true, out CommandType commandType))
        {
            throw new CommandParserException($"Unknown command: '{command}'");
        }

        return new Command(commandType, commandValue);
    }
}
