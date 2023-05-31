using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class HyperLinkTests
{
    [Fact]
    public void MustRenderCorrectly()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HyperLink hyperLink = new();
        page.Controls.Add(hyperLink);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal("<a ></a>", result);
    }
    
    [Fact]
    public void MustRenderCorrectly__WithTextAndUrl()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HyperLink hyperLink = new()
        {
            NavigateUrl = "http://example.com",
            Text = "Example.com"
        };
        page.Controls.Add(hyperLink);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"<a  href=""http://example.com"">Example.com</a>", result);
    }
}