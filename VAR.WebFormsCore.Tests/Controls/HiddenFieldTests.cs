using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class HiddenFieldTests
{
    [Fact]
    public void MustRenderCorrectly()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HiddenField hiddenField = new();
        page.Controls.Add(hiddenField);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"<input type=""hidden""  id=""ctl00"" name=""ctl00""></input>", result);
    }

    [Fact]
    public void MustRenderCorrectly__WithValue()
    {
        string value = "Test";
        FakeWebContext fakeWebContext = new();
        Page page = new();
        HiddenField hiddenField = new() { Value = value };
        page.Controls.Add(hiddenField);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(value, hiddenField.Value);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"<input type=""hidden""  id=""ctl00"" name=""ctl00"" value=""Test""></input>", result);
    }

    [Fact]
    public void MustRenderCorrectly__WithChangedValue()
    {
        string value = "Test";
        string changedValue = "Changed";
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        HiddenField hiddenField = new() { Value = value };
        page.Controls.Add(hiddenField);

        fakeWebContext.RequestForm.Add(hiddenField.ClientID, changedValue);
        page.ProcessRequest(fakeWebContext);

        Assert.Equal(changedValue, hiddenField.Value);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(@"<input type=""hidden""  id=""ctl00"" name=""ctl00"" value=""Changed""></input>", result);
    }
}