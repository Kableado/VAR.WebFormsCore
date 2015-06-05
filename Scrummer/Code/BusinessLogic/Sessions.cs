using System;
using System.Collections.Generic;
using System.Web;
using Scrummer.Code.Entities;

namespace Scrummer.Code.BusinessLogic
{
    public class Sessions
    {
        #region declarations

        private static Sessions _currentInstance = null;

        private List<Session> _sessions = new List<Session>();

        private string _cookieName = "ScrummerSID";
        private int _cookieExpirationDays = 30;

        #endregion

        #region Properties

        public static Sessions Current
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = new Sessions();
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

        #endregion
        
        #region Life cycle

        public Sessions()
        {
            LoadData();
        }

        #endregion

        #region Public methods

        public void Session_SetCookie(HttpContext context, Session session)
        {
            HttpCookie cookie = new HttpCookie(_cookieName, session.SessionToken);
            cookie.Expires = DateTime.Now.AddDays(_cookieExpirationDays);
            context.Response.Cookies.Add(cookie);
        }

        public bool Session_Init(HttpContext context, string userName)
        {
            lock (_sessions)
            {
                var session = new Session();
                session.UserName = userName;
                session.SessionToken = CryptoUtils.GetCryptoToken();
                session.StartDate = DateTime.UtcNow;
                _sessions.Add(session);

                Session_SetCookie(context, session);

                SaveData();
            }
            return true;
        }

        public Session Session_GetCurrent(HttpContext context)
        {
            HttpCookie cookie = context.Request.Cookies[_cookieName];
            if (cookie == null) { return null; }

            string sessionToken = cookie.Value;
            if (string.IsNullOrEmpty(sessionToken)) { return null; }

            Session session = Session_GetByToken(sessionToken);
            return session;
        }

        public bool Session_FinalizeCurrent(HttpContext context)
        {
            lock (_sessions)
            {
                Session session = Session_GetCurrent(context);
                if (session == null) { return false; }

                if (_sessions.Remove(session) == false) { return false; }

                HttpCookie cookie = new HttpCookie(_cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1d);
                context.Response.Cookies.Add(cookie);

                SaveData();
            }
            return true;
        }

        #endregion

        #region Private methods

        private Session Session_GetByToken(string sessionToken)
        {
            foreach (Session session in _sessions)
            {
                if (session.SessionToken == sessionToken)
                {
                    return session;
                }
            }
            return null;
        }

        #region Persistence

        private const string PersistenceFile = "priv/sessions.json";

        private void LoadData()
        {
            _sessions = Persistence.LoadList<Session>(PersistenceFile);
        }

        private void SaveData()
        {
            Persistence.SaveList(PersistenceFile, _sessions);
        }

        #endregion

        #endregion
    }
}