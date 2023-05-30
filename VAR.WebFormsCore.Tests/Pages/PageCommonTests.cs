using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Pages;
using VAR.WebFormsCore.Tests.Fakes;
using Xunit;

namespace VAR.WebFormsCore.Tests.Pages;

public class PageCommonTests
{
    #region ProcessRequest TestForm

    private class TestEmptyForm : PageCommon
    {
        public TestEmptyForm(bool mustBeAuthenticated)
        {
            MustBeAuthenticated = mustBeAuthenticated;
        }
    }

    [Fact]
    public void ProcessRequest__TestEmptyForm()
    {
        FakeWebContext fakeWebContext = new();
        TestEmptyForm testEmptyForm = new(mustBeAuthenticated: false);

        testEmptyForm.ProcessRequest(fakeWebContext);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        string result = fakeWebContext.FakeWritePackages.ToString("");
        Assert.Equal(
            expected: """
                        <!DOCTYPE html>
                        <html ><head ><meta  content="IE=Edge" http-equiv="X-UA-Compatible" /><meta  content="text/html; charset=utf-8" http-equiv="content-type" /><meta  name="author" /><meta  name="Copyright" /><meta  name="viewport" content="width=device-width, initial-scale=1, maximum-scale=4, user-scalable=1" /><script type="text/javascript" src="ScriptsBundler?v=1.0.0.0"></script>
                        <link href="StylesBundler?v=1.0.0.0" type="text/css" rel="stylesheet"/>
                        </head><body ><form  id="formMain" name="formMain" method="post" action="TestEmptyForm"><div  class="divHeader"><a  href="."><h1 ></h1></a><input type="submit"  id="ctl00_btnPostback" name="ctl00_btnPostback" class="button" style="display: none;" value="Postback"></input><div  class="divUserInfo"></div></div><div  class="divContent"></div></form></body></html>
                        """,
            actual: result);
    }

    [Fact]
    public void ProcessRequest__TestEmptyFormNotAuthenticated__RedirectToFrmLogin()
    {
        string loginHandler = "FrmLogin";
        (GlobalConfig.Get() as FakeGlobalConfig)?.FakeSetLoginHandler(loginHandler);
        FakeWebContext fakeWebContext = new();
        TestEmptyForm testEmptyForm = new(mustBeAuthenticated: true);

        testEmptyForm.ProcessRequest(fakeWebContext);

        Assert.Equal(302, fakeWebContext.ResponseStatusCode);
        Assert.Equal(loginHandler, fakeWebContext.FakeResponseHeaders["location"]);
    }

    [Fact]
    public void ProcessRequest__TestEmptyFormPostClickLogout__RedirectToFrmLogin()
    {
        string loginHandler = "FrmLogin";
        (GlobalConfig.Get() as FakeGlobalConfig)?.FakeSetLoginHandler(loginHandler);
        (GlobalConfig.Get() as FakeGlobalConfig)?.FakeSetAuthenticated(true);
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        fakeWebContext.RequestForm.Add("ctl00_ctl02_btnLogout", "Logout");
        TestEmptyForm testEmptyForm = new(mustBeAuthenticated: true);

        testEmptyForm.ProcessRequest(fakeWebContext);

        Assert.Equal(302, fakeWebContext.ResponseStatusCode);
        Assert.Equal(loginHandler, fakeWebContext.FakeResponseHeaders["location"]);
    }

    #endregion ProcessRequest TestForm
}