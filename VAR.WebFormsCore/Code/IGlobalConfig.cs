using System.Collections.Generic;

namespace VAR.WebFormsCore.Code;

public interface IGlobalConfig
{
    string Title { get; }
    string TitleSeparator { get; }
    string Author { get; }
    string Copyright { get; }
    string DefaultHandler { get; }
    string LoginHandler { get; }
    List<string> AllowedExtensions { get; }

    bool IsUserAuthenticated(IWebContext context);
    void UserDeauthenticate(IWebContext context);
}