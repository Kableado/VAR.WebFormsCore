using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class ButtonTests
{
    [Fact]
    public void MustRenderCorrectly()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        Button button = new();
        page.Controls.Add(button);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"<input type=""submit""  class=""button"" value=""""></input>", result);
    }

    [Fact]
    public void MustRenderCorrectly__WithOnClientClick()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        Button button = new()
        {
            OnClientClick = "alert(1)",
        };
        page.Controls.Add(button);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"<input type=""submit""  class=""button"" value="""" onclick=""alert(1)""></input>", result);
    }

    [Fact]
    public void MustRenderCorrectly__ClickWithCommandArgument()
    {
        string commandArgument = "Test";
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        Button button = new()
        {
            CommandArgument = commandArgument,
        };
        string? result = null;
        button.Click += (o, _) => { result = (o as Button)?.CommandArgument; };
        page.Controls.Add(button);

        fakeWebContext.RequestForm.Add(button.ClientID, "Clicked");
        page.ProcessRequest(fakeWebContext);

        Assert.Equal(commandArgument, result);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
    }
}