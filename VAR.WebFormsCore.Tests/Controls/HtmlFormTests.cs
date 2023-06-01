using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class HtmlFormTests
{
    [Fact]
    public void MustRenderCorrectly()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HtmlForm htmlForm = new();
        page.Controls.Add(htmlForm);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"<form  method=""post"" action=""Page""></form>", result);
    }

    [Fact]
    public void MustRenderCorrectly__WithQueryParameters()
    {
        FakeWebContext fakeWebContext = new();
        fakeWebContext.RequestQuery.Add("test", "value");
        Page page = new();
        HtmlForm htmlForm = new();
        page.Controls.Add(htmlForm);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"<form  method=""post"" action=""Page?&amp;test=value""></form>", result);
    }
}