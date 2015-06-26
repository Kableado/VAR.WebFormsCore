using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using VAR.Focus.Web.Code.BusinessLogic;
using VAR.Focus.Web.Code.Entities;
using VAR.Focus.Web.Code.JSON;

namespace VAR.Focus.Web.Controls
{
    public class CardBoardHandler : IHttpHandler
    {
        #region Declarations

        private static object _monitor = new object();
        private static Dictionary<int, CardBoard> _cardBoards = new Dictionary<int, CardBoard>();

        #endregion

        #region IHttpHandler

        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.RequestType == "GET")
                {
                    if (string.IsNullOrEmpty(GetRequestParm(context, "IDCardEvent")))
                    {
                        ProcessInitializationReciver(context);
                    }
                    else
                    {
                        ProcessEventReciver(context);
                    }
                }
                if (context.Request.RequestType == "POST")
                {
                    ProcessEventSender(context);
                }
            }
            catch (Exception ex)
            {
                ResponseObject(context, new OperationStatus { IsOK = false, Message = ex.Message, });
            }
        }

        #endregion

        #region Private methods

        private void ProcessInitializationReciver(HttpContext context)
        {
            int idBoard = Convert.ToInt32(GetRequestParm(context, "IDBoard"));
            CardBoard cardBoard;
            if (_cardBoards.ContainsKey(idBoard) == false)
            {
                lock (_cardBoards)
                {
                    if (_cardBoards.ContainsKey(idBoard) == false)
                    {
                        cardBoard = new CardBoard(idBoard);
                        _cardBoards[idBoard] = cardBoard;
                    }
                }
            }

            if (_cardBoards.ContainsKey(idBoard))
            {
                cardBoard = _cardBoards[idBoard];
                List<Card> listCards = cardBoard.Cards_Status();
                List<ICardEvent> listEvents = new List<ICardEvent>();
                int lastIDCardEvent = cardBoard.GetLastIDCardEvent();
                int lastIDCard = cardBoard.GetLastIDCard();
                if (listCards.Count > 0)
                {
                    listEvents = CardBoard.ConvertCardsToEvents(listCards, lastIDCardEvent);
                }
                else
                {
                    listEvents = new List<ICardEvent>();
                }
                ResponseObject(context, listEvents);
            }
        }

        private CardBoard GetCardBoard(int idBoard)
        {
            CardBoard cardBoard = null;
            if (_cardBoards.ContainsKey(idBoard) == false)
            {
                lock (_cardBoards)
                {
                    if (_cardBoards.ContainsKey(idBoard) == false)
                    {
                        cardBoard = new CardBoard(idBoard);
                        _cardBoards[idBoard] = cardBoard;
                    }
                }
            }
            cardBoard = _cardBoards[idBoard];
            return cardBoard;
        }

        private void ProcessEventReciver(HttpContext context)
        {
            int idBoard = Convert.ToInt32(GetRequestParm(context, "IDBoard"));
            int idCardEvent = Convert.ToInt32(GetRequestParm(context, "IDCardEvent"));
            string strTimePoolData = GetRequestParm(context, "TimePoolData");
            int timePoolData = Convert.ToInt32(string.IsNullOrEmpty(strTimePoolData) ? "0" : strTimePoolData);

            CardBoard cardBoard = GetCardBoard(idBoard);
            bool mustWait = (timePoolData > 0);
            do
            {
                List<ICardEvent> listMessages = cardBoard.Cards_GetEventList(idCardEvent);
                if (listMessages.Count > 0)
                {
                    mustWait = false;
                    ResponseObject(context, listMessages);
                    return;
                }

                if (mustWait)
                {
                    lock (_monitor) { Monitor.Wait(_monitor, timePoolData); }
                }
            } while (mustWait);
            ResponseObject(context, new List<Message>());
        }

        private void ProcessEventSender(HttpContext context)
        {
            Session session = Sessions.Current.Session_GetCurrent(context);
            string currentUserName = session.UserName;
            string strIDBoard = GetRequestParm(context, "IDBoard");
            int idBoard = Convert.ToInt32(string.IsNullOrEmpty(strIDBoard) ? "0" : strIDBoard);
            string command = GetRequestParm(context, "Command");
            int idCard = 0;
            bool done = false;
            CardBoard cardBoard = GetCardBoard(idBoard);
            lock (cardBoard)
            {
                if (command == "Create")
                {
                    string title = GetRequestParm(context, "Title");
                    string body = GetRequestParm(context, "Body");
                    int x = Convert.ToInt32(GetRequestParm(context, "X"));
                    int y = Convert.ToInt32(GetRequestParm(context, "Y"));
                    idCard = cardBoard.Card_Create(title, body, x, y, currentUserName);
                    done = true;
                }
                if (command == "Move")
                {
                    idCard = Convert.ToInt32(GetRequestParm(context, "IDCard"));
                    int x = Convert.ToInt32(GetRequestParm(context, "X"));
                    int y = Convert.ToInt32(GetRequestParm(context, "Y"));
                    cardBoard.Card_Move(idCard, x, y, currentUserName);
                    done = true;
                }
                if (command == "Edit")
                {
                    idCard = Convert.ToInt32(GetRequestParm(context, "IDCard"));
                    string title = GetRequestParm(context, "Title");
                    string body = GetRequestParm(context, "Body");
                    cardBoard.Card_Edit(idCard, title, body, currentUserName);
                    done = true;
                }
                if (command == "Delete")
                {
                    idCard = Convert.ToInt32(GetRequestParm(context, "IDCard"));
                    cardBoard.Card_Delete(idCard, currentUserName);
                    done = true;
                }
            }
            if (done)
            {
                NotifyAll();
                ResponseObject(context, new OperationStatus
                {
                    IsOK = true,
                    Message = "Update successfully",
                    ReturnValue = Convert.ToString(idCard)
                });
            }
        }

        private void NotifyAll()
        {
            lock (_monitor) { Monitor.PulseAll(_monitor); }
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