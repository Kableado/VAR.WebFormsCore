using Xunit;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Tests.Code;

public class ServerHelperTests
{
    #region MapContentPath
    
    [Fact]
    public void MapContentPath__Empty__Slash()
    {
        ServerHelpers.SetContentRoot(string.Empty);
        string result = ServerHelpers.MapContentPath(string.Empty);
        
        Assert.Equal("/", result);
    }
    
    [Fact]
    public void MapContentPath__File__SlashFile()
    {
        ServerHelpers.SetContentRoot(string.Empty);
        string result = ServerHelpers.MapContentPath("file.ext");
        
        Assert.Equal("/file.ext", result);
    }
    
    [Fact]
    public void MapContentPath__FileWithRoot__AbsolutePathToFile()
    {
        ServerHelpers.SetContentRoot("/opt/App");
        string result = ServerHelpers.MapContentPath("file.ext");
        
        Assert.Equal("/opt/App/file.ext", result);
    }
    
    #endregion MapContentPath
    
    #region HtmlEncode
    
    [Fact]
    public void HtmlEncode__Empty__Empty()
    {
        string result = ServerHelpers.HtmlEncode(string.Empty);
        
        Assert.Equal(string.Empty, result);
    }
    
    [Fact]
    public void HtmlEncode__SafeString__SameString()
    {
        string text = "aA0,     ()!?=\\-_*+";
        
        string result = ServerHelpers.HtmlEncode(text);
        
        Assert.Equal(text, result);
    }
    
    [Fact]
    public void HtmlEncode__UnsafeString__SafeString()
    {
        string text = "<<>>\"'&\u00FF";
        string safeText = "&lt;&lt;&gt;&gt;&quot;&#39;&amp;&#255;";
        
        string result = ServerHelpers.HtmlEncode(text);
        
        Assert.Equal(safeText, result);
    }

    #endregion HtmlEncode
    
    #region UrlEncode
    
    [Fact]
    public void UrlEncode__Empty__Empty()
    {
        string result = ServerHelpers.UrlEncode(string.Empty);
        
        Assert.Equal(string.Empty, result);
    }
    
    [Fact]
    public void UrlEncode__SafeString__SameString()
    {
        string text = "aA0()!-_*";
        
        string result = ServerHelpers.UrlEncode(text);
        
        Assert.Equal(text, result);
    }
    
    [Fact]
    public void UrlEncode__UnsafeString__SafeString()
    {
        string text = "<< >>\"' + &\u00FF";
        string safeText = "%3C%3C+%3E%3E%22%27+%2B+%26%FF";
        
        string result = ServerHelpers.UrlEncode(text);
        
        Assert.Equal(safeText, result);
    }

    #endregion UrlEncode
}