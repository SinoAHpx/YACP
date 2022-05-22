using System.Runtime.Serialization;

namespace YACP.Models.Exceptions;

[Serializable]
public class CommandAttributeMissingException : Exception
{
    public CommandAttributeMissingException()
    {
    }

    public CommandAttributeMissingException(string message) : base(message)
    {
    }

    public CommandAttributeMissingException(string message, Exception inner) : base(message, inner)
    {
    }

    protected CommandAttributeMissingException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}