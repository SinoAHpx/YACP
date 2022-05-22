namespace YACP.Models;

public class ArgumentAttribute : Attribute
{
    public string[] Identifiers { get; set; }

    public string? Description { get; set; }

    public object? DefaultValue { get; set; }

    public bool IsRequired { get; set; } = false;

    public bool IsDefault { get; set; } = false;

    public ArgumentAttribute(params string[] identifiers)
    {
        Identifiers = identifiers;
    }
}