using Xunit;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Tests.Code;

public class UnitTests
{
    [Fact]
    public void ToString__100px__100px()
    {
        Unit unit = new(100, UnitType.Pixel);

        string result = unit.ToString();

        Assert.Equal("100px", result);
    }

    [Fact]
    public void ToString__50px__50px()
    {
        Unit unit = new(100, UnitType.Pixel);

        string result = unit.ToString();

        Assert.Equal("100px", result);
    }

    [Fact]
    public void ToString__100pc__100pc()
    {
        Unit unit = new(100, UnitType.Percentage);

        string result = unit.ToString();

        Assert.Equal("100%", result);
    }

    [Fact]
    public void ToString__50pc__50pc()
    {
        Unit unit = new(100, UnitType.Percentage);

        string result = unit.ToString();

        Assert.Equal("100%", result);
    }

    [Fact]
    public void ToString__100UnknownUnits__Empty()
    {
        Unit unit = new(100, (UnitType)1000);

        string result = unit.ToString();

        Assert.Equal(string.Empty, result);
    }
}