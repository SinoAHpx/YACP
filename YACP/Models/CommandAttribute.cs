namespace YACP.Models;

/// <summary>
/// Indicating a class is a command entity.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class CommandAttribute : Attribute
{
    /// <summary>
    /// The prefix of the command.
    /// </summary>
    public string? Prefix { get; set; } = "/";

    /// <summary>
    /// The names of the command.
    /// </summary>
    public string[] Names { get; set; }

    public string? Description { get; set; }

    public CommandAttribute(params string[] names)
    {
        Names = names;
    }
}