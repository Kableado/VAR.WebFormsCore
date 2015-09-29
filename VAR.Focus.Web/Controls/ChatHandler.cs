using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using VAR.Focus.Web.Code;
using VAR.Focus.Web.Code.BusinessLogic;
using VAR.Focus.Web.Code.Entities;

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
            try
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
            catch (Exception ex)
            {
                context.ResponseObject(new OperationStatus { IsOK = false, Message = ex.Message, });
            }
        }

        #endregion

        #region Private methods

        private void ProcessReciver(HttpContext context)
        {
            string idMessageBoard = context.GetRequestParm("IDMessageBoard");
            int idMessage = Convert.ToInt32(context.GetRequestParm("IDMessage"));
            string strTimePoolData = context.GetRequestParm("TimePoolData");
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
                        context.ResponseObject(listMessages);
                        return;
                    }
                }
                if (mustWait)
                {
                    lock (_monitor) { Monitor.Wait(_monitor, timePoolData); }
                }
            } while (mustWait);
            context.ResponseObject(new List<Message>());
        }

        private void ProcessSender(HttpContext context)
        {
            string text = Convert.ToString(context.GetRequestParm("Text"));
            string idMessageBoard = context.GetRequestParm("IDMessageBoard");
            if (string.IsNullOrEmpty(idMessageBoard)) { idMessageBoard = "root"; }
            string userName = Convert.ToString(context.GetRequestParm("UserName"));
            Session session = Sessions.Current.Session_GetCurrent(context);
            if (session.UserName.ToLower() != userName.ToLower())
            {
                context.ResponseObject(new OperationStatus { IsOK = false, Message = "User mismatch" });
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
            context.ResponseObject(new OperationStatus { IsOK = true, Message = "Message sent" });
        }

        #endregion
    }
}
