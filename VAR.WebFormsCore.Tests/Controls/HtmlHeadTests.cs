using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class HtmlHeadTests
{
    [Fact]
    public void MustRenderCorrectly()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HtmlHead htmlHead = new();
        page.Controls.Add(htmlHead);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal("<head ></head>", result);
    }
    
    [Fact]
    public void MustRenderCorrectlyWithTitle()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HtmlHead htmlHead = new();
        htmlHead.Title = "Test";
        page.Controls.Add(htmlHead);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal("<head ><title>Test</title></head>", result);
    }
    
    [Fact]
    public void MustRenderCorrectlyWithMeta()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HtmlHead htmlHead = new();
        page.Controls.Add(htmlHead);
        HtmlMeta htmlMeta = new()
        {
            Name = "TestMeta",
            Content = "TestMetaContent",
            HttpEquiv = "TestMetaHttpEquiv"
        };
        htmlHead.Controls.Add(htmlMeta);
        
        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <head ><meta  name="TestMeta" content="TestMetaContent" http-equiv="TestMetaHttpEquiv" /></head>
                        """,
            actual: result);
    }
}