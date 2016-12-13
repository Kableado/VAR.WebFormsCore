using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using VAR.Focus.BusinessLogic;
using VAR.Focus.BusinessLogic.Entities;
using VAR.Focus.Web.Code;

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
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.RequestType == "GET")
                {
                    if (string.IsNullOrEmpty(context.GetRequestParm("IDCardEvent")))
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
                context.ResponseObject(new OperationStatus { IsOK = false, Message = ex.Message, });
            }
        }

        #endregion

        #region Private methods

        private void ProcessInitializationReciver(HttpContext context)
        {
            int idBoard = Convert.ToInt32(context.GetRequestParm("IDBoard"));
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
                context.ResponseObject(listEvents);
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
            int idBoard = Convert.ToInt32(context.GetRequestParm("IDBoard"));
            int idCardEvent = Convert.ToInt32(context.GetRequestParm("IDCardEvent"));
            string strTimePoolData = context.GetRequestParm("TimePoolData");
            int timePoolData = Convert.ToInt32(string.IsNullOrEmpty(strTimePoolData) ? "0" : strTimePoolData);

            CardBoard cardBoard = GetCardBoard(idBoard);
            bool mustWait = (timePoolData > 0);
            do
            {
                List<ICardEvent> listMessages = cardBoard.Cards_GetEventList(idCardEvent);
                if (listMessages.Count > 0)
                {
                    mustWait = false;
                    context.ResponseObject(listMessages);
                    return;
                }

                if (mustWait)
                {
                    lock (_monitor) { Monitor.Wait(_monitor, timePoolData); }
                }
            } while (mustWait);
            context.ResponseObject(new List<Message>());
        }

        private void ProcessEventSender(HttpContext context)
        {
            Session session = WebSessions.Current.Session_GetCurrent(context);
            string currentUserName = session.UserName;
            string strIDBoard = context.GetRequestParm("IDBoard");
            int idBoard = Convert.ToInt32(string.IsNullOrEmpty(strIDBoard) ? "0" : strIDBoard);
            string command = context.GetRequestParm("Command");
            int idCard = 0;
            bool done = false;
            CardBoard cardBoard = GetCardBoard(idBoard);
            lock (cardBoard)
            {
                if (command == "Create")
                {
                    string title = context.GetRequestParm("Title");
                    string body = context.GetRequestParm("Body");
                    int x = Convert.ToInt32(context.GetRequestParm("X"));
                    int y = Convert.ToInt32(context.GetRequestParm("Y"));
                    idCard = cardBoard.Card_Create(title, body, x, y, currentUserName);
                    done = true;
                }
                if (command == "Move")
                {
                    idCard = Convert.ToInt32(context.GetRequestParm("IDCard"));
                    int x = Convert.ToInt32(context.GetRequestParm("X"));
                    int y = Convert.ToInt32(context.GetRequestParm("Y"));
                    cardBoard.Card_Move(idCard, x, y, currentUserName);
                    done = true;
                }
                if (command == "Edit")
                {
                    idCard = Convert.ToInt32(context.GetRequestParm("IDCard"));
                    string title = context.GetRequestParm("Title");
                    string body = context.GetRequestParm("Body");
                    cardBoard.Card_Edit(idCard, title, body, currentUserName);
                    done = true;
                }
                if (command == "Delete")
                {
                    idCard = Convert.ToInt32(context.GetRequestParm("IDCard"));
                    cardBoard.Card_Delete(idCard, currentUserName);
                    done = true;
                }
            }
            if (done)
            {
                NotifyAll();
                context.ResponseObject(new OperationStatus
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

        #endregion
    }
}
