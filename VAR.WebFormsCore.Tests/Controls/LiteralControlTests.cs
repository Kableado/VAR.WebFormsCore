using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class LiteralControlTests
{
    [Fact]
    public void MustRenderCorrectly__Empty()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        LiteralControl literalControl = new();
        page.Controls.Add(literalControl);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"", result);
    }
    
    [Fact]
    public void MustRenderCorrectly__AnyContent()
    {
        string anyContent = "AnyContent<script>alert(1)</script>";
        FakeWebContext fakeWebContext = new();
        Page page = new();
        LiteralControl literalControl = new(anyContent);
        page.Controls.Add(literalControl);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(anyContent, result);
    }
}