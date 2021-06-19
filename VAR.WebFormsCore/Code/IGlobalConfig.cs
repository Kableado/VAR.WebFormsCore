using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace VAR.WebForms.Common.Code
{
    public interface IGlobalConfig
    {
        string Title { get; }
        string TitleSeparator { get; }
        string Author { get; }
        string Copyright { get; }
        string DefaultHandler { get; }
        string LoginHandler { get; }
        List<string> AllowedExtensions { get; }

        bool IsUserAuthenticated(HttpContext context);
        void UserUnauthenticate(HttpContext context);
    }
}
