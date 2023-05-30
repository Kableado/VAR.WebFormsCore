using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class HtmlGenericControlTests
{
    [Fact]
    public void MustRenderCorrectly()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HtmlGenericControl htmlGenericControl = new("test");
        page.Controls.Add(htmlGenericControl);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal("<test ></test>", result);
    }
}