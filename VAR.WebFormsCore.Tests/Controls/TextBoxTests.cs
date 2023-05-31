using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class TextBoxTests
{
    [Fact]
    public void MustRenderCorrectly__Normal()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        TextBox textBox = new();
        page.Controls.Add(textBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="text"  id="ctl00" name="ctl00"></input>
                        """,
            actual: result);
    }
    
    [Fact]
    public void MustRenderCorrectly__NormalWithText()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        TextBox textBox = new() { Text = "Text", };
        page.Controls.Add(textBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="text"  id="ctl00" name="ctl00" value="Text"></input>
                        """,
            actual: result);
    }
    
    [Fact]
    public void MustRenderCorrectly__Password()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        TextBox textBox = new() {TextMode = TextBoxMode.Password, };
        page.Controls.Add(textBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="password"  id="ctl00" name="ctl00"></input>
                        """,
            actual: result);
    }
    
    [Fact]
    public void MustRenderCorrectly__PasswordWithText()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        TextBox textBox = new() { TextMode = TextBoxMode.Password, Text = "Password", };
        page.Controls.Add(textBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="password"  id="ctl00" name="ctl00" value="Password"></input>
                        """,
            actual: result);
    }
    
    [Fact]
    public void MustRenderCorrectly__MultiLine()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        TextBox textBox = new() {TextMode = TextBoxMode.MultiLine, };
        page.Controls.Add(textBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <textarea  id="ctl00" name="ctl00"></textarea>
                        """,
            actual: result);
    }
    
    [Fact]
    public void MustRenderCorrectly__MultiLineWithText()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        TextBox textBox = new() { TextMode = TextBoxMode.MultiLine, Text = "Multi\nLine", };
        page.Controls.Add(textBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <textarea  id="ctl00" name="ctl00">Multi
                        Line</textarea>
                        """,
            actual: result);
    }
    
    [Fact]
    public void MustRenderCorrectly__WithChangedText()
    {
        string text = "Test";
        string changedValue = "Changed";
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        TextBox textBox = new() { Text = text };
        page.Controls.Add(textBox);

        fakeWebContext.RequestForm.Add(textBox.ClientID, changedValue);
        page.ProcessRequest(fakeWebContext);

        Assert.Equal(changedValue, textBox.Text);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="text"  id="ctl00" name="ctl00" value="Changed"></input>
                        """,
            actual: result);
    }
}