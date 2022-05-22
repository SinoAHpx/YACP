using System.Linq;
using Xunit;
using YACP.Models;
using YACP.Utils;

namespace YACP.Test;

public class StringUtilsTests
{
    
    [Fact]
    public void GetIdIndex()
    {
        var raw = "/help -cmd 123 --cmd awd -v".Split();

        Assert.Equal(1, raw.GetIdentifierIndex(new ArgumentAttribute("-cmd")).Item2);
        Assert.Equal(3, raw.GetIdentifierIndex(new ArgumentAttribute("--cmd")).Item2);
        Assert.Equal(5, raw.GetIdentifierIndex(new ArgumentAttribute("-v")).Item2);
    }
}