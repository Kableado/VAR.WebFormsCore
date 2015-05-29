using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using Scrummer.Code.JSON;

namespace Scrummer.Code
{
    public class Message
    {
        public int IDMessage { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    };

    public class MessageBoard
    {
        private List<Message> _messages = new List<Message>();
        private int lastIDMessage = 0;

        public List<Message> Messages_GetList(int idMessage)
        {
            List<Message> listMessages = new List<Message>();
            for (int i = 0, n = _messages.Count; i < n; i++)
            {
                Message msg = _messages[i];
                if (msg.IDMessage > idMessage)
                {
                    listMessages.Insert(0, msg);
                }
                else
                {
                    break;
                }
            }
            return listMessages;
        }

        public void Message_Add(string userName, string text)
        {
            lastIDMessage++;
            Message msg = new Message();
            msg.IDMessage = lastIDMessage;
            msg.UserName = userName;
            msg.Text = text;
            msg.Date = DateTime.UtcNow;
            _messages.Insert(0, msg);
        }
    }

    public class ChatHandler : IHttpHandler
    {
        private static object _monitor = new object();
        private static Dictionary<int, MessageBoard> _chatBoards = new Dictionary<int, MessageBoard>();

        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.RequestType == "GET")
            {
                ProcessRevicer(context);
            }
            if (context.Request.RequestType == "POST")
            {
                ProcessSender(context);
            }
        }

        private void ProcessRevicer(HttpContext context)
        {
            int idBoard = Convert.ToInt32(context.Request.Params["idBoard"]);
            int idMessage = Convert.ToInt32(context.Request.Params["idMessage"]);
            MessageBoard messageBoard;
            bool mustWait = true;
            do
            {
                if (_chatBoards.ContainsKey(idBoard))
                {
                    messageBoard = _chatBoards[idBoard];
                    List<Message> listMessages = messageBoard.Messages_GetList(idMessage);
                    if (listMessages.Count > 0)
                    {
                        mustWait = false;
                        ResponseObject(context, listMessages);
                    }
                }
                if (mustWait)
                {
                    lock (_monitor) { Monitor.Wait(_monitor, 10000); }
                }
            } while (mustWait);
        }

        private void ProcessSender(HttpContext context)
        {
            string strIDBoard = GetRequestParm(context, "idBoard");
            int idBoard = Convert.ToInt32(string.IsNullOrEmpty(strIDBoard) ? "0" : strIDBoard);
            string userName = Convert.ToString(GetRequestParm(context, "userName"));
            string text = Convert.ToString(GetRequestParm(context, "text"));

            lock (_chatBoards)
            {
                MessageBoard messageBoard;
                if (_chatBoards.ContainsKey(idBoard))
                {
                    messageBoard = _chatBoards[idBoard];
                }
                else
                {
                    messageBoard = new MessageBoard();
                    _chatBoards[idBoard] = messageBoard;
                }
                messageBoard.Message_Add(userName, text);
                lock (_monitor) { Monitor.PulseAll(_monitor); }
            }
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
            context.Response.Write(jsonWritter.Write(obj));
        }
    }
}
