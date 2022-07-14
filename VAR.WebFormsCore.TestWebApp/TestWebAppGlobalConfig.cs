using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.TestWebApp;

public class TestWebAppGlobalConfig: IGlobalConfig
{ 
    public string Title { get; } = "TestWebApp";
    public string TitleSeparator { get; } = " :: ";
    public string Author { get; } = "XXX";
    public string Copyright { get; } = "Copyright (c) 2022 by XXX, All Right Reserved";
    public string DefaultHandler { get; } = nameof(FrmDefault);
    public string LoginHandler { get; }
    public List<string> AllowedExtensions { get; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".ico", ".wav", ".mp3", ".ogg", ".mp4", ".webm", ".webp", ".mkv", ".avi" };
    public bool IsUserAuthenticated(HttpContext context)
    {
        return false;
    }

    public void UserUnauthenticate(HttpContext context)
    {
    }
}