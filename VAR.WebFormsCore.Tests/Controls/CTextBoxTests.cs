using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Controls;

public class CTextBoxTests
{
    #region MustRenderCorrectly

    [Fact]
    public void MustRenderCorrectly()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        CTextBox cTextBox = new();
        page.Controls.Add(cTextBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="text"  id="ctl00_ctl00" name="ctl00_ctl00" class="textBox" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);"></input>
                        """,
            actual: result);
    }

    [Fact]
    public void MustRenderCorrectly__WithMostProperties()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        CTextBox cTextBox = new()
        {
            CssClassExtra = "extraClass",
            PlaceHolder = "Placeholder",
            MarkedInvalid = true,
            TextMode = TextBoxMode.Normal,
            AllowEmpty = true,
            KeepSize = true,
            Text = "Test",
        };
        page.Controls.Add(cTextBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="text"  id="ctl00_ctl00" name="ctl00_ctl00" class="textBox extraClass textBoxInvalid" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);" placeholder="Placeholder" value="Test"></input>
                        """,
            actual: result);
    }

    [Fact]
    public void MustRenderCorrectly__WithNextFocusOnEnter()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        CTextBox cTextBox = new();
        page.Controls.Add(cTextBox);
        CTextBox cTextBox2 = new();
        page.Controls.Add(cTextBox2);
        cTextBox.NextFocusOnEnter = cTextBox2;

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="text"  id="ctl00_ctl00" name="ctl00_ctl00" class="textBox" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);" onkeydown="if(event.keyCode==13){document.getElementById(&#39;ctl01&#39;).focus(); return false;}"></input><input type="text"  id="ctl01_ctl00" name="ctl01_ctl00" class="textBox" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);"></input>
                        """,
            actual: result);
    }

    [Fact]
    public void MustRenderCorrectly__WithChangedText()
    {
        string text = "Test";
        string changedValue = "Changed";

        FakeWebContext fakeWebContext0 = new();
        Page page0 = new();
        CTextBox cTextBox0 = new() { Text = text };
        page0.Controls.Add(cTextBox0);
        page0.ProcessRequest(fakeWebContext0);

        FakeWebContext fakeWebContext1 = new(requestMethod: "POST");
        fakeWebContext1.RequestForm.Add(cTextBox0.TxtContent.ClientID, changedValue);
        Page page1 = new();
        CTextBox cTextBox1 = new() { Text = text };
        page1.Controls.Add(cTextBox1);
        page1.ProcessRequest(fakeWebContext1);

        Assert.Equal(changedValue, cTextBox1.Text);
        Assert.Equal(200, fakeWebContext1.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext1.ResponseContentType);
        string result = fakeWebContext1.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <input type="text"  id="ctl00_ctl00" name="ctl00_ctl00" class="textBox" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);" value="Changed"></input>
                        """,
            actual: result);
    }

    [Fact]
    public void MustRenderCorrectly_Multiline()
    {
        FakeWebContext fakeWebContext = new();
        Page page = new();
        CTextBox cTextBox = new()
        {
            TextMode = TextBoxMode.MultiLine,
            KeepSize = true,
        };
        page.Controls.Add(cTextBox);

        page.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <textarea  id="ctl00_ctl00" name="ctl00_ctl00" class="textBox" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);"></textarea><input type="hidden"  id="ctl00_ctl01" name="ctl00_ctl01"></input><script>
                        var ctl00_cfg = { "txtContent": "ctl00_ctl00", "hidSize": "ctl00_ctl01", "keepSize": true };
                        CTextBox_Multiline_Init(ctl00_cfg);
                        </script>
                        
                        """,
            actual: result);
    }

    [Fact]
    public void MustRenderCorrectly_Multiline__GetClientsideHeight__Null()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        CTextBox cTextBox = new()
        {
            TextMode = TextBoxMode.MultiLine,
            KeepSize = true,
        };
        page.Controls.Add(cTextBox);

        page.ProcessRequest(fakeWebContext);
        int? resultHeight = cTextBox.GetClientsideHeight();

        Assert.Null(resultHeight);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <textarea  id="ctl00_ctl00" name="ctl00_ctl00" class="textBox" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);"></textarea><input type="hidden"  id="ctl00_ctl01" name="ctl00_ctl01"></input><script>
                        var ctl00_cfg = { "txtContent": "ctl00_ctl00", "hidSize": "ctl00_ctl01", "keepSize": true };
                        CTextBox_Multiline_Init(ctl00_cfg);
                        </script>
                        
                        """,
            actual: result);
    }

    [Fact]
    public void MustRenderCorrectly_Multiline__GetAndSetClientsideHeight__100()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        CTextBox cTextBox = new()
        {
            TextMode = TextBoxMode.MultiLine,
            KeepSize = true,
        };
        page.Controls.Add(cTextBox);
        Button button = new();
        button.Click += (_, _) =>
        {
            cTextBox.SetClientsideHeight(10);
            cTextBox.SetClientsideHeight(100);
        };
        page.Controls.Add(button);

        fakeWebContext.RequestForm.Add(button.ClientID, "Clicked");
        page.ProcessRequest(fakeWebContext);
        int? resultHeight = cTextBox.GetClientsideHeight();

        Assert.Equal(100, resultHeight);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <textarea  id="ctl00_ctl00" name="ctl00_ctl00" class="textBox" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);"></textarea><input type="hidden"  id="ctl00_ctl01" name="ctl00_ctl01" value="{ &quot;height&quot;: 100, &quot;width&quot;: null, &quot;scrollTop&quot;: null }"></input><script>
                        var ctl00_cfg = { "txtContent": "ctl00_ctl00", "hidSize": "ctl00_ctl01", "keepSize": true };
                        CTextBox_Multiline_Init(ctl00_cfg);
                        </script>
                        <input type="submit"  class="button" value=""></input>
                        """,
            actual: result);
    }

    [Fact]
    public void MustRenderCorrectly_Multiline__GetAndSetClientsideHeight__Null()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        CTextBox cTextBox = new()
        {
            TextMode = TextBoxMode.MultiLine,
            KeepSize = true,
        };
        page.Controls.Add(cTextBox);
        Button button = new();
        button.Click += (_, _) => { cTextBox.SetClientsideHeight(null); };
        page.Controls.Add(button);

        fakeWebContext.RequestForm.Add(button.ClientID, "Clicked");
        page.ProcessRequest(fakeWebContext);
        int? resultHeight = cTextBox.GetClientsideHeight();

        Assert.Null(resultHeight);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <textarea  id="ctl00_ctl00" name="ctl00_ctl00" class="textBox" onchange="ElementRemoveClass(this, &#39;textBoxInvalid&#39;);"></textarea><input type="hidden"  id="ctl00_ctl01" name="ctl00_ctl01"></input><script>
                        var ctl00_cfg = { "txtContent": "ctl00_ctl00", "hidSize": "ctl00_ctl01", "keepSize": true };
                        CTextBox_Multiline_Init(ctl00_cfg);
                        </script>
                        <input type="submit"  class="button" value=""></input>
                        """,
            actual: result);
    }

    [Fact]
    public void MustRenderCorrectly_Multiline__GetClientsideHeightInjectArray__Null()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        CTextBox cTextBox = new()
        {
            TextMode = TextBoxMode.MultiLine,
            KeepSize = true,
        };
        page.Controls.Add(cTextBox);
        Button button = new();
        button.Click += (_, _) =>
        {
            if (cTextBox.HidSize != null) { cTextBox.HidSize.Value = "[]"; }
        };
        page.Controls.Add(button);

        fakeWebContext.RequestForm.Add(button.ClientID, "Clicked");
        page.ProcessRequest(fakeWebContext);
        int? resultHeight = cTextBox.GetClientsideHeight();

        Assert.Null(resultHeight);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
    }

    [Fact]
    public void MustRenderCorrectly_Multiline__GetClientsideHeightInjectObject__Null()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        Page page = new();
        CTextBox cTextBox = new()
        {
            TextMode = TextBoxMode.MultiLine,
            KeepSize = true,
        };
        page.Controls.Add(cTextBox);
        Button button = new();
        button.Click += (_, _) =>
        {
            if (cTextBox.HidSize != null) { cTextBox.HidSize.Value = "{}"; }
        };
        page.Controls.Add(button);

        fakeWebContext.RequestForm.Add(button.ClientID, "Clicked");
        page.ProcessRequest(fakeWebContext);
        int? resultHeight = cTextBox.GetClientsideHeight();

        Assert.Null(resultHeight);
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal("text/html", fakeWebContext.ResponseContentType);
    }

    #endregion MustRenderCorrectly

    #region IsEmpty

    [Fact]
    public void IsEmpty__Empty__True()
    {
        CTextBox cTextBox = new() { Text = string.Empty, };

        bool result = cTextBox.IsEmpty();

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty__Text__False()
    {
        CTextBox cTextBox = new() { Text = "Text", };

        bool result = cTextBox.IsEmpty();

        Assert.False(result);
    }

    #endregion IsEmpty

    #region IsValid

    [Fact]
    public void IsEmpty__EmptyAllowEmptyFalse__False()
    {
        CTextBox cTextBox = new() { Text = string.Empty, AllowEmpty = false, };

        bool result = cTextBox.IsValid();

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty__TextAllowEmptyFalse__True()
    {
        CTextBox cTextBox = new() { Text = "Text", AllowEmpty = false, };

        bool result = cTextBox.IsValid();

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty__EmptyAllowEmptyTrue__True()
    {
        CTextBox cTextBox = new() { Text = string.Empty, AllowEmpty = true, };

        bool result = cTextBox.IsValid();

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty__TextAllowEmptyTrue__True()
    {
        CTextBox cTextBox = new() { Text = "Text", AllowEmpty = true, };

        bool result = cTextBox.IsValid();

        Assert.True(result);
    }

    #endregion IsEmpty
}