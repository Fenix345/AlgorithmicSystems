using AlgorithmicSystems.Enums;

namespace AlgorithmicSystems.Data;

internal sealed class Command
{
    internal Command(CommandType type, string value)
    {
        Type = type;
        Value = value;
    }

    internal CommandType Type { get; init; }
    internal string Value { get; init; }
}
