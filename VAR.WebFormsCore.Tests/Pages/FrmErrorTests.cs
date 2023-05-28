using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Pages;

public class FrmErrorTests
{
    [Fact]
    public void ProcessRequest__TestException()
    {
        FakeWebContext fakeWebContext = new();
        FrmError frmError = new(new Exception("Test"));
        
        frmError.ProcessRequest(fakeWebContext);

        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected:   """
                        <!DOCTYPE html>
                        <html ><head ><title>Application Error</title><meta  content="IE=Edge" http-equiv="X-UA-Compatible" /><meta  content="text/html; charset=utf-8" http-equiv="content-type" /><meta  name="author" /><meta  name="Copyright" /><meta  name="viewport" content="width=device-width, initial-scale=1, maximum-scale=4, user-scalable=1" /><script type="text/javascript" src="ScriptsBundler?v=1.0.0.0"></script>
                        <link href="StylesBundler?v=1.0.0.0" type="text/css" rel="stylesheet"/>
                        </head><body ><form  id="formMain" name="formMain" method="post" action="FrmError"><div  class="divHeader"><a  href="."><h1 ></h1></a><input type="submit"  id="ctl00_btnPostback" name="ctl00_btnPostback" class="button" style="display: none;" value="Postback"></input><div  class="divUserInfo"></div></div><div  class="divContent"><h2 >Application Error</h2><p><b>Message:</b> Test</p><p><b>Stacktrace:</b></p><div  class="divCode"><pre><code></code></pre></div></div></form></body></html>
                        """,
            actual: result);
    }

}