using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using Scrummer.Code.BusinessLogic;
using Scrummer.Code.Entities;
using Scrummer.Code.JSON;

namespace Scrummer.Controls
{
    public class ChatHandler : IHttpHandler
    {
        #region Declarations

        private static object _monitor = new object();
        private static Dictionary<int, MessageBoard> _chatBoards = new Dictionary<int, MessageBoard>();

        #endregion

        #region IHttpHandler

        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
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
            int idBoard = Convert.ToInt32(GetRequestParm(context, "IDBoard"));
            int idMessage = Convert.ToInt32(GetRequestParm(context, "IDMessage"));
            string strTimePoolData = GetRequestParm(context, "TimePoolData");
            int timePoolData = Convert.ToInt32(string.IsNullOrEmpty(strTimePoolData) ? "0" : strTimePoolData);

            MessageBoard messageBoard;
            bool mustWait = (timePoolData > 0);
            do
            {
                if (_chatBoards.ContainsKey(idBoard) == false)
                {
                    lock (_chatBoards)
                    {
                        if (_chatBoards.ContainsKey(idBoard) == false)
                        {
                            messageBoard = new MessageBoard(idBoard);
                            _chatBoards[idBoard] = messageBoard;
                        }
                    }
                }

                if (_chatBoards.ContainsKey(idBoard))
                {
                    messageBoard = _chatBoards[idBoard];
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
            string strIDBoard = GetRequestParm(context, "IDBoard");
            int idBoard = Convert.ToInt32(string.IsNullOrEmpty(strIDBoard) ? "0" : strIDBoard);
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
                if (_chatBoards.ContainsKey(idBoard))
                {
                    messageBoard = _chatBoards[idBoard];
                }
                else
                {
                    messageBoard = new MessageBoard(idBoard);
                    _chatBoards[idBoard] = messageBoard;
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
