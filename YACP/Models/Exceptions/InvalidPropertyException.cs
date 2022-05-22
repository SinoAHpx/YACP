using System.Runtime.Serialization;

namespace YACP.Models.Exceptions;

[Serializable]
public class InvalidPropertyException : Exception
{
    public InvalidPropertyException()
    {
    }

    public InvalidPropertyException(string message) : base(message)
    {
    }

    public InvalidPropertyException(string message, Exception inner) : base(message, inner)
    {
    }

    protected InvalidPropertyException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}