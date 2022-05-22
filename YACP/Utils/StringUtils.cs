using YACP.Models;

namespace YACP.Utils;

public static class StringUtils
{
    /// <summary>
    /// Find where's first index of a substring in a string, if not found returns ("", -1)
    /// </summary>
    /// <param name="splitCmd"></param>
    /// <param name="argumentAttribute"></param>
    /// <returns></returns>
    public static (string, int) GetIdentifierIndex(this string[] splitCmd, ArgumentAttribute argumentAttribute)
    {
        foreach (var identifier in argumentAttribute.Identifiers)
        {
            var index = splitCmd.ToList().IndexOf(identifier);
            if (index != -1)
            {
                return (identifier, index);
            }
        }

        return ("", -1);
    }
}