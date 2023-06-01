using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Pages;

public class PageTests
{
    [Fact]
    public void ProcessRequest__Empty__Empty()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ProcessRequest__SendResponseOnPreInit__NullContentType()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        page.PreInit += (_, _) => { fakeWebContext.FakeSetResponseHasStarted(true); };

        page.ProcessRequest(fakeWebContext);

        Assert.Null(fakeWebContext.ResponseContentType);
    }

    [Fact]
    public void ProcessRequest__SendResponseOnInit__NullContentType()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        page.Init += (_, _) => { fakeWebContext.FakeSetResponseHasStarted(true); };

        page.ProcessRequest(fakeWebContext);

        Assert.Null(fakeWebContext.ResponseContentType);
    }

    [Fact]
    public void ProcessRequest__SendResponseOnLoad__NullContentType()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        page.Load += (_, _) => { fakeWebContext.FakeSetResponseHasStarted(true); };

        page.ProcessRequest(fakeWebContext);

        Assert.Null(fakeWebContext.ResponseContentType);
    }

    [Fact]
    public void ProcessRequest__SendResponseOnPreRender__NullContentType()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        page.PreRender += (_, _) => { fakeWebContext.FakeSetResponseHasStarted(true); };

        page.ProcessRequest(fakeWebContext);

        Assert.Null(fakeWebContext.ResponseContentType);
    }

    [Fact]
    public void ProcessRequest__AbortIfIsPostbackOnGet__Empty()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "GET");
        Page page = new();
        page.Load += (_, _) =>
        {
            if (page.IsPostBack) { fakeWebContext.FakeSetResponseHasStarted(true); }
        };

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ProcessRequest__AbortIfIsPostbackOnPost__NullContentType()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        page.Load += (_, _) =>
        {
            if (page.IsPostBack) { fakeWebContext.FakeSetResponseHasStarted(true); }
        };

        page.ProcessRequest(fakeWebContext);

        Assert.Null(fakeWebContext.ResponseContentType);
    }

    [Fact]
    public void ProcessRequest__ThrowException__NullContentType()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        page.Load += (_, _) => throw new Exception("Test");

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
    }
}