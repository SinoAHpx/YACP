# YACP

Yet another command parser...but simple one.

## Glimpse

YACP is not powerful.

### Supported types

- string
- int, int?
- double, double?
- bool
- List, Array, IEnumerable of int, double and string

### Expected syntax

Unlike other command parsers, YACP is not dedicatedly design for a command line app, and it also does not strictly follow the rule of POSIX command line. YACP needs a raw input command and a CLR typed entity (class, record, struct...etc), the CLR type uses diverse attribute to represent the command.

Here is a very standard expected command for YACP: `/help -a1 p1 -a2 222 -a3`, and a very standard command entity:

```cs
[Command("help", "h", Prefix = "/")]
class Help
{
    [Argument("-a1")]
    public string P1 { get; set; }

    [Argument("-a2")]
    public int P2 { get; set; }

    [Argument("-a3")]
    public bool P3 { get; set; }
}
```

- `/help`: used for identify if a string is a command, and it consists of 2 segments: prefix and **names**, which means you can have many names for a command, suchlike `/h` and `/help` are equivalent.
- `-a1...-a2...-a3`: they are arguments identifiers, multiple arguments identifiers is also supported in YACP, noticeablly a argument identifier is simply consists of a string instead of a combination of prefix and names like above.
- `...p1...222...`: parameters can have various types, here, `p1` is considered as a string, `222` is an int, but for `-a3`, which supposed to be a bool, won't have a value but up to whether it exists to be true or false.

There's also some tricks in YACP, for an example: `/h foo bar far boo`, apparently it's an invalid POSIX command but it can be entitized to following entity in YACP:

```cs
[Command("help", "h", Prefix = "/")]
class Help
{
    [Argument("-a1", IsDefault = true)]
    public string P1 { get; set; }
}
```

Besides `IsDefault`, there's also other tricks like `DefaultValue` and `IsRequired`. Most of time, they cannot be stand together.
