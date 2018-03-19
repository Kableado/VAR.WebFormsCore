using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using VAR.Focus.BusinessLogic;
using VAR.Focus.BusinessLogic.Entities;
using VAR.Focus.BusinessLogic.Persistence;
using VAR.Focus.Web.Code;

namespace VAR.Focus.Web.Controls
{
    public class HndChat : IHttpHandler
    {
        #region Declarations

        private const int MaxWaitLoops = 5;

        private static object _monitor = new object();
        private static Dictionary<string, MessageBoard> _chatBoards = new Dictionary<string, MessageBoard>();

        #endregion Declarations

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

        #endregion IHttpHandler

        #region Private methods

        private void ProcessReciver(HttpContext context)
        {
            string idMessageBoard = context.GetRequestParm("IDMessageBoard");
            int idMessage = Convert.ToInt32(context.GetRequestParm("IDMessage"));
            string strTimePoolData = context.GetRequestParm("TimePoolData");
            int timePoolData = Convert.ToInt32(string.IsNullOrEmpty(strTimePoolData) ? "0" : strTimePoolData);

            MessageBoard messageBoard;
            int waitCount = (timePoolData > 0) ? MaxWaitLoops : 0;
            do
            {
                if (_chatBoards.ContainsKey(idMessageBoard) == false)
                {
                    lock (_chatBoards)
                    {
                        if (_chatBoards.ContainsKey(idMessageBoard) == false)
                        {
                            messageBoard = new MessageBoard(idMessageBoard, new JsonFilePersistence());
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
                        waitCount = 0;
                        context.ResponseObject(listMessages);
                        return;
                    }
                }
                if (waitCount > 0)
                {
                    lock (_monitor) { Monitor.Wait(_monitor, timePoolData); }
                    waitCount--;
                }
            } while (waitCount > 0);
            context.ResponseObject(new List<Message>());
        }

        private void ProcessSender(HttpContext context)
        {
            string text = Convert.ToString(context.GetRequestParm("Text"));
            string idMessageBoard = context.GetRequestParm("IDMessageBoard");
            if (string.IsNullOrEmpty(idMessageBoard)) { idMessageBoard = "root"; }
            string userName = Convert.ToString(context.GetRequestParm("UserName"));
            Session session = WebSessions.Current.Session_GetCurrent(context);
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
                    messageBoard = new MessageBoard(idMessageBoard, new JsonFilePersistence());
                    _chatBoards[idMessageBoard] = messageBoard;
                }
                messageBoard.Message_Add(userName, text);
                lock (_monitor) { Monitor.PulseAll(_monitor); }
            }
            context.ResponseObject(new OperationStatus { IsOK = true, Message = "Message sent" });
        }

        #endregion Private methods
    }
}