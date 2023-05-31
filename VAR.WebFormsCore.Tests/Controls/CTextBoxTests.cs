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