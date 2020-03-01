using System.Collections.Generic;
using System.Web;
using VAR.Focus.Web.Code;
using VAR.Focus.Web.Pages;
using VAR.WebForms.Common.Code;

namespace VAR.Focus.Web
{
    public class FocusGlobalConfig : IGlobalConfig
    {
        public string Title { get; } = "Focus";
        public string TitleSeparator { get; } = " :: ";
        public string Author { get; } = "Valeriano Alfonso Rodriguez";
        public string Copyright { get; } = "Copyright (c) 2015-2018 by Valeriano Alfonso, All Right Reserved";
        public string DefaultHandler { get; } = nameof(FrmBoard);
        public string LoginHandler { get; } = nameof(FrmLogin);
        public List<string> AllowedExtensions { get; } = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".ico", ".wav", ".mp3", ".ogg", ".mp4", ".webm", ".webp", ".mkv", ".avi" };

        public bool IsUserAuthenticated(HttpContext context)
        {
            return WebSessions.Current.Session_IsUserAuthenticated(context);
        }

        public void UserUnauthenticate(HttpContext context)
        {
            WebSessions.Current.Session_FinalizeCurrent(context);
        }
    }
}