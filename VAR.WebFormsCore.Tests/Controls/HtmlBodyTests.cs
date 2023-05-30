using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class HtmlBodyTests
{
    [Fact]
    public void MustRenderCorrectly()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HtmlBody htmlBody = new();
        page.Controls.Add(htmlBody);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal("<body ></body>", result);
    }
}