using System.Reflection;
using YACP.Models;
using YACP.Models.Exceptions;
using YACP.Utils;

namespace YACP;

public static class Parser
{
    public static T Parse<T>(this string command) where T : new()
    {
        command = command
            .Replace('\r', ' ')
            .Replace('\n', ' ')
            .Replace('\t', ' ')
            .Trim();

        var splitCommand = SplitCommand(command);
        var type = typeof(T);
        var commandEntity = Activator.CreateInstance<T>();
        var commandAttribute = GetCommandAttribute(type);
        var arguments = MapArguments(type);

        foreach (var (propertyInfo, argumentAttribute) in arguments)
        {
            var (matchedIdentifier, matchedIndex) = splitCommand.GetIdentifierIndex(argumentAttribute);
            if (matchedIndex == -1)
            {
                commandEntity = HandleMissingProperty<T>(splitCommand, propertyInfo, argumentAttribute);
                continue;
            }

            void AssignValue()
            {
                if (matchedIndex == splitCommand.Length - 1)
                {
                    throw new InvalidCommandException($"Missing value for argument {matchedIdentifier}");
                }

                var nextValue = splitCommand[matchedIndex + 1];
                if (IsArgument(nextValue, arguments))
                {
                    throw new InvalidCommandException($"Missing value for argument {matchedIdentifier}");
                }

                if (propertyInfo.IsStringProperty())
                {
                    propertyInfo.SetValue(commandEntity,
                        string.Join(' ', GetMiddleValues(matchedIndex + 1, splitCommand, arguments)));
                }

                if (propertyInfo.IsDoubleProperty())
                {
                    try
                    {
                        propertyInfo.SetValue(commandEntity, double.Parse(nextValue));
                    }
                    catch (Exception e)
                    {
                        throw new InvalidCommandException($"Invalid value for argument {matchedIdentifier}", e);
                    }
                }
                
                if (propertyInfo.IsIntProperty())
                {
                    try
                    {
                        propertyInfo.SetValue(commandEntity, int.Parse(nextValue));
                    }
                    catch (Exception e)
                    {
                        throw new InvalidCommandException($"Invalid value for argument {matchedIdentifier}", e);
                    }
                }
            }

            void AssignSequence()
            {
                if (matchedIndex == splitCommand.Length - 1)
                {
                    throw new InvalidCommandException($"Missing value for argument {matchedIdentifier}");
                }

                var nextValue = splitCommand[matchedIndex + 1];
                if (IsArgument(nextValue, arguments))
                {
                    throw new InvalidCommandException($"Missing value for argument {matchedIdentifier}");
                }

                if (propertyInfo.IsStringListProperty() || propertyInfo.IsStringEnumerableProperty())
                {
                    propertyInfo.SetValue(commandEntity, GetMiddleValues(matchedIndex + 1, splitCommand, arguments));
                }

                if (propertyInfo.IsStringArrayProperty())
                {
                    propertyInfo.SetValue(commandEntity,
                        GetMiddleValues(matchedIndex + 1, splitCommand, arguments).ToArray());
                }

                if (propertyInfo.IsIntListProperty() || propertyInfo.IsIntEnumerableProperty())
                {
                    try
                    {
                        propertyInfo.SetValue(commandEntity,
                            GetMiddleValues(matchedIndex + 1, splitCommand, arguments)
                                .Select(int.Parse).ToList());
                    }
                    catch (Exception e)
                    {
                        throw new InvalidCommandException("Invalid value for argument, suppose to be int", e);
                    }
                    
                }
                
                if (propertyInfo.IsIntArrayProperty())
                {
                    try
                    {
                        propertyInfo.SetValue(commandEntity,
                            GetMiddleValues(matchedIndex + 1, splitCommand, arguments)
                                .Select(int.Parse).ToArray());
                    }
                    catch (Exception e)
                    {
                        throw new InvalidCommandException("Invalid value for argument, suppose to be int", e);
                    }
                }
                
                if (propertyInfo.IsDoubleListProperty() || propertyInfo.IsDoubleEnumerableProperty())
                {
                    try
                    {
                        propertyInfo.SetValue(commandEntity,
                            GetMiddleValues(matchedIndex + 1, splitCommand, arguments)
                                .Select(double.Parse).ToList());
                    }
                    catch (Exception e)
                    {
                        throw new InvalidCommandException("Invalid value for argument, suppose to be double", e);
                    }
                }
                
                if (propertyInfo.IsDoubleArrayProperty())
                {
                    try
                    {
                        propertyInfo.SetValue(commandEntity,
                            GetMiddleValues(matchedIndex + 1, splitCommand, arguments)
                                .Select(double.Parse).ToArray());
                    }
                    catch (Exception e)
                    {
                        throw new InvalidCommandException("Invalid value for argument, suppose to be double", e);
                    }
                }
            }

            if (propertyInfo.IsValueProperty())
            {
                AssignValue();
            }

            if (propertyInfo.IsSequenceProperty())
            {
                AssignSequence();
            }

            if (propertyInfo.IsOptionProperty())
            {
                propertyInfo.SetValue(commandEntity, true);
            }
        }

        return commandEntity;
    }

    private static List<string> GetMiddleValues(int currentIndex, string[] splitCommand, List<(PropertyInfo, ArgumentAttribute)> arguments)
    {
        var re = new List<string>();
        if (!IsArgument(splitCommand[currentIndex], arguments))
        {
            re.Add(splitCommand[currentIndex]);
            if (currentIndex != splitCommand.Length - 1)
            {
                re.AddRange(GetMiddleValues(currentIndex + 1, splitCommand, arguments));
            }
        }

        return re;
    }

    private static bool IsArgument(string item,  List<(PropertyInfo, ArgumentAttribute)> arguments)
    {
        return arguments.Any(x => x.Item2.Identifiers.Any(z => z == item));
    }
    
    private static T HandleMissingProperty<T>(string[] splitCommand, PropertyInfo propertyInfo,
        ArgumentAttribute argumentAttribute)
    {
        if (argumentAttribute.IsRequired && (argumentAttribute.IsDefault || argumentAttribute.DefaultValue != null))
        {
            throw new InvalidEntityException(
                $"{nameof(argumentAttribute.IsRequired)}, {nameof(argumentAttribute.IsDefault)}, {nameof(argumentAttribute.DefaultValue)} cannot be set at same time");
        }

        var commandEntity = Activator.CreateInstance<T>();
        if (argumentAttribute.IsRequired)
        {
            throw new InvalidCommandException($"Missing required argument: {propertyInfo.Name}");
        }

        if (argumentAttribute.DefaultValue != null)
        {
            propertyInfo.SetValue(commandEntity, argumentAttribute.DefaultValue);
        }

        if (argumentAttribute.IsDefault)
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(commandEntity, string.Join(' ', splitCommand[1..]));
                return commandEntity;
            }

            if (propertyInfo.PropertyType == typeof(List<string>))
            {
                propertyInfo.SetValue(commandEntity, splitCommand[1..].ToList());
                return commandEntity;
            }

            throw new InvalidCommandException(
                $"Property {propertyInfo.Name} is not a string or string list thus cannot be set to default value");
        }

        return commandEntity;
    }

    private static List<(PropertyInfo, ArgumentAttribute)> MapArguments(Type type)
    {
        var properties = type.GetProperties();

        if (properties.Length == 0)
        {
            throw new InvalidEntityException($"No properties found in {type.Name}");
        }

        var re = properties
            .Select(x => (x, x.GetCustomAttribute<ArgumentAttribute>()))
            .Where(x => x.Item2 != null)
            .ToList()!;

        if (re.Count == 0)
        {
            throw new InvalidEntityException($"No properties with ArgumentAttribute found in {type.Name}");
        }

        return re!;
    }

    private static string[] SplitCommand(string rawCommand)
    {
        var split = rawCommand.Split(' ');
        if (split.Length < 2)
        {
            throw new InvalidCommandException();
        }

        return split;
    }

    private static CommandAttribute GetCommandAttribute(Type type)
    {
        var commandAttribute = type.GetCustomAttribute<CommandAttribute>();
        if (commandAttribute == null)
        {
            throw new CommandAttributeMissingException();
        }

        return commandAttribute;
    }
}