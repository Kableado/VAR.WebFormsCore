using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Pages;

public class FrmEchoTests
{
    [Fact]
    public void ProcessRequest__Empty__Empty()
    {
        FakeWebContext fakeWebContext = new();
        FrmEcho frmEcho = new();
        
        frmEcho.ProcessRequest(fakeWebContext);

        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected:   """
                        <pre><code>Header:{ }
                        Query:{ }
                        Form:{ }
                        </code></pre>
                        """,
            actual: result);
    }
    
    [Fact]
    public void ProcessRequest__OneQueryParameterGet__FormData()
    {
        FakeWebContext fakeWebContext = new();
        fakeWebContext.RequestQuery.Add("Test", "Value");
        FrmEcho frmEcho = new();
        
        frmEcho.ProcessRequest(fakeWebContext);

        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected:   """
                        <pre><code>Header:{ }
                        Query:{ "Test": "Value" }
                        Form:{ }
                        </code></pre>
                        """,
            actual: result);
    }
    
    [Fact]
    public void ProcessRequest__OneFormParameterPost__FormData()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        fakeWebContext.RequestForm.Add("Test", "Value");
        FrmEcho frmEcho = new();
        
        frmEcho.ProcessRequest(fakeWebContext);

        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected:   """
                        <pre><code>Header:{ }
                        Query:{ }
                        Form:{ "Test": "Value" }
                        </code></pre>
                        """,
            actual: result);
    }
}