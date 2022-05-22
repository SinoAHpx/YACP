using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using YACP.Models.Exceptions;
using YACP.Utils;

namespace YACP.Test;

public class UtilsTests
{
    #region ingredient

    class AllValueBean
    {
        public string Str { get; set; }

        public int Int { get; set; }

        public double Double { get; set; }

        public string? NullableStr { get; set; }

        public int NullableInt { get; set; }
        
        public double? NullableDouble { get; set; }
    }

    class OptionBean
    {
        public bool Option { get; set; }

        public bool? NullableOption { get; set; }
    }

    class SequenceBean
    {
        public List<string> List1 { get; set; }

        public string[] List3 { get; set; }

        public IEnumerable<string> List4 { get; set; }

        public List<int> List5 { get; set; }

        public int[] List7 { get; set; }

        public IEnumerable<int> List8 { get; set; }
    }

    #endregion

    [Fact]
    public void ThrowExceptionWhenNull()
    {
        Assert.Throws<InvalidPropertyException>(() =>
        {
            typeof(AllValueBean).GetProperty("imnotexist").IsValueProperty();
            typeof(AllValueBean).GetProperty("imnotexist").IsSequenceProperty();
            typeof(AllValueBean).GetProperty("imnotexist").IsOptionProperty();
        });
    }
    
    [Fact]
    public void IsValueProperty()
    {
        var type = typeof(AllValueBean);
        Assert.True(type.GetProperties().All(p => p.IsValueProperty()));

    }

    [Fact]
    public void IsOptionProperty()
    {
        var type = typeof(OptionBean);
        Assert.True(type.GetProperty("Option").IsOptionProperty());
        Assert.False(type.GetProperty("NullableOption").IsOptionProperty());
    }

    [Fact]
    public void IsSequenceProperty()
    {
        var type = typeof(SequenceBean);

        Assert.True(type.GetProperties().All(p => p.IsSequenceProperty()));
    }
}