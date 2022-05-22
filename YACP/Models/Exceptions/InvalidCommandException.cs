using System.Runtime.Serialization;

namespace YACP.Models.Exceptions;

[Serializable]
public class InvalidCommandException : Exception
{
    public InvalidCommandException()
    {
    }

    public InvalidCommandException(string message) : base(message)
    {
    }

    public InvalidCommandException(string message, Exception inner) : base(message, inner)
    {
    }

    protected InvalidCommandException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}