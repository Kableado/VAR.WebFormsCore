using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using VAR.Focus.Web.Code.BusinessLogic;
using VAR.Focus.Web.Code.Entities;
using VAR.Focus.Web.Code.JSON;

namespace VAR.Focus.Web.Controls
{
    public class ChatHandler : IHttpHandler
    {
        #region Declarations

        private static object _monitor = new object();
        private static Dictionary<string, MessageBoard> _chatBoards = new Dictionary<string, MessageBoard>();

        #endregion

        #region IHttpHandler

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.RequestType == "GET")
            {
                ProcessReciver(context);
            }
            if (context.Request.RequestType == "POST")
            {
                ProcessSender(context);
            }
        }

        #endregion

        #region Private methods

        private void ProcessReciver(HttpContext context)
        {
            string idMessageBoard = GetRequestParm(context, "IDMessageBoard");
            int idMessage = Convert.ToInt32(GetRequestParm(context, "IDMessage"));
            string strTimePoolData = GetRequestParm(context, "TimePoolData");
            int timePoolData = Convert.ToInt32(string.IsNullOrEmpty(strTimePoolData) ? "0" : strTimePoolData);

            MessageBoard messageBoard;
            bool mustWait = (timePoolData > 0);
            do
            {
                if (_chatBoards.ContainsKey(idMessageBoard) == false)
                {
                    lock (_chatBoards)
                    {
                        if (_chatBoards.ContainsKey(idMessageBoard) == false)
                        {
                            messageBoard = new MessageBoard(idMessageBoard);
                            _chatBoards[idMessageBoard] = messageBoard;
                        }
                    }
                }

                if (_chatBoards.ContainsKey(idMessageBoard))
                {
                    messageBoard = _chatBoards[idMessageBoard];
                    List<Message> listMessages = messageBoard.Messages_GetList(idMessage);
                    if (listMessages.Count > 0)
                    {
                        mustWait = false;
                        ResponseObject(context, listMessages);
                        return;
                    }
                }
                if (mustWait)
                {
                    lock (_monitor) { Monitor.Wait(_monitor, timePoolData); }
                }
            } while (mustWait);
            ResponseObject(context, new List<Message>());
        }

        private void ProcessSender(HttpContext context)
        {
            string text = Convert.ToString(GetRequestParm(context, "Text"));
            string idMessageBoard = GetRequestParm(context, "IDMessageBoard");
            if (string.IsNullOrEmpty(idMessageBoard)) { idMessageBoard = "root"; }
            string userName = Convert.ToString(GetRequestParm(context, "UserName"));
            Session session = Sessions.Current.Session_GetCurrent(context);
            if (session.UserName.ToLower() != userName.ToLower())
            {
                ResponseObject(context, new OperationStatus { IsOK = false, Message = "User mismatch" });
                return;
            }

            lock (_chatBoards)
            {
                MessageBoard messageBoard;
                if (_chatBoards.ContainsKey(idMessageBoard))
                {
                    messageBoard = _chatBoards[idMessageBoard];
                }
                else
                {
                    messageBoard = new MessageBoard(idMessageBoard);
                    _chatBoards[idMessageBoard] = messageBoard;
                }
                messageBoard.Message_Add(userName, text);
                lock (_monitor) { Monitor.PulseAll(_monitor); }
            }
            ResponseObject(context, new OperationStatus { IsOK = true, Message = "Message sent" });
        }

        private string GetRequestParm(HttpContext context, string parm)
        {
            foreach (string key in context.Request.Params.AllKeys)
            {
                if (string.IsNullOrEmpty(key) == false && key.EndsWith(parm))
                {
                    return context.Request.Params[key];
                }
            }
            return string.Empty;
        }

        private void ResponseObject(HttpContext context, object obj)
        {
            var jsonWritter = new JSONWriter(true);
            context.Response.ContentType = "text/json";
            string strObject = jsonWritter.Write(obj);
            context.Response.Write(strObject);
        }

        #endregion
    }
}
