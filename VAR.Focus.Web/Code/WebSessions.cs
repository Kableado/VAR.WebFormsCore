using System;
using System.Web;
using VAR.Focus.BusinessLogic;
using VAR.Focus.BusinessLogic.Entities;

namespace VAR.Focus.Web.Code
{
    public class WebSessions
    {
        #region Declarations

        private static WebSessions _currentInstance = null;

        private string _cookieName = "FocusSID";
        private int _cookieExpirationDays = 30;

        #endregion Declarations

        #region Properties

        public static WebSessions Current
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = new WebSessions();
                }
                return _currentInstance;
            }
            set { _currentInstance = value; }
        }

        public string CookieName
        {
            get { return _cookieName; }
            set { _cookieName = value; }
        }

        public int CookieExpirationDays
        {
            get { return _cookieExpirationDays; }
            set { _cookieExpirationDays = value; }
        }

        #endregion Properties

        #region Public methods

        public void Session_SetCookie(HttpContext context, Session session)
        {
            HttpCookie cookie = new HttpCookie(_cookieName, session.SessionToken);
            cookie.Expires = DateTime.Now.AddDays(_cookieExpirationDays);
            context.Response.Cookies.Add(cookie);
        }

        public void Session_Init(HttpContext context, string userName)
        {
            Session session = Sessions.Current.Session_Create(userName);
            Session_SetCookie(context, session);
        }

        public Session Session_GetCurrent(HttpContext context)
        {
            HttpCookie cookie = context.Request.Cookies[_cookieName];
            if (cookie == null) { return null; }

            string sessionToken = cookie.Value;
            if (string.IsNullOrEmpty(sessionToken)) { return null; }

            Session session = Sessions.Current.Session_GetByToken(sessionToken);
            return session;
        }

        public bool Session_FinalizeCurrent(HttpContext context)
        {
            Session session = Session_GetCurrent(context);
            if (Sessions.Current.Session_Delete(session) == false) { return false; }

            HttpCookie cookie = new HttpCookie(_cookieName);
            cookie.Expires = DateTime.Now.AddDays(-1d);
            context.Response.Cookies.Add(cookie);

            return true;
        }

        #endregion Public methods
    }
}