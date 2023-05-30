using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Tests.Fakes;

public class FakeGlobalConfig : IGlobalConfig
{
    public string Title => string.Empty;

    public string TitleSeparator => string.Empty;

    public string Author => string.Empty;

    public string Copyright => string.Empty;

    public string DefaultHandler => string.Empty;

    private string _loginHandler = string.Empty;

    public void FakeSetLoginHandler(string loginHandler)
    {
        _loginHandler = loginHandler;
    }

    public string LoginHandler => _loginHandler;

    public List<string> AllowedExtensions { get; } = new();

    private bool _authenticated;

    public void FakeSetAuthenticated(bool authenticated)
    {
        _authenticated = authenticated;
    }
    
    public bool IsUserAuthenticated(IWebContext context)
    {
        return _authenticated;
    }

    public void UserDeauthenticate(IWebContext context)
    {
        _authenticated = false;
    }
}