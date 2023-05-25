using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.TestWebApp;

public class TestWebAppGlobalConfig : IGlobalConfig
{
    public string Title => "TestWebApp";
    public string TitleSeparator => " :: ";
    public string Author => "XXX";
    public string Copyright => "Copyright (c) 2022 by XXX, All Right Reserved";
    public string DefaultHandler => nameof(FrmDefault);
    public string LoginHandler => nameof(FrmDefault);

    public List<string> AllowedExtensions { get; } = new()
        { ".png", ".jpg", ".jpeg", ".gif", ".ico", ".wav", ".mp3", ".ogg", ".mp4", ".webm", ".webp", ".mkv", ".avi" };

    public bool IsUserAuthenticated(HttpContext context)
    {
        return false;
    }

    public void UserUnauthenticate(HttpContext context)
    {
    }
}