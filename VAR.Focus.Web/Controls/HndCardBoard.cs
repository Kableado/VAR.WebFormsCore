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
    public class HndCardBoard : IHttpHandler
    {
        #region Declarations

        private const int MaxWaitLoops = 5;

        private static object _monitor = new object();
        private static Dictionary<int, CardBoard> _cardBoards = new Dictionary<int, CardBoard>();

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

        #endregion IHttpHandler

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
                        cardBoard = new CardBoard(idBoard, new JsonFilePersistence());
                        _cardBoards[idBoard] = cardBoard;
                    }
                }
            }

            if (_cardBoards.ContainsKey(idBoard))
            {
                cardBoard = _cardBoards[idBoard];
                List<Card> listCards = cardBoard.Cards_Status();
                List<Region> listRegions = cardBoard.Regions_Status();
                List<IBoardEvent> listEvents = new List<IBoardEvent>();
                int lastIDCardEvent = cardBoard.GetLastIDCardEvent();
                int lastIDCard = cardBoard.GetLastIDCard();
                listEvents = new List<IBoardEvent>();
                if (listRegions.Count > 0)
                {
                    listEvents.AddRange(CardBoard.ConvertRegionsToEvents(listRegions, lastIDCardEvent));
                }
                if (listCards.Count > 0)
                {
                    listEvents.AddRange(CardBoard.ConvertCardsToEvents(listCards, lastIDCardEvent));
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
                        cardBoard = new CardBoard(idBoard, new JsonFilePersistence());
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
            int waitCount = (timePoolData > 0) ? MaxWaitLoops : 0;
            do
            {
                List<IBoardEvent> listMessages = cardBoard.Cards_GetEventList(idCardEvent);
                if (listMessages.Count > 0)
                {
                    waitCount = 0;
                    context.ResponseObject(listMessages);
                    return;
                }

                if (waitCount > 0)
                {
                    lock (_monitor) { Monitor.Wait(_monitor, timePoolData); }
                    waitCount--;
                }
            } while (waitCount > 0);
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
            int idRegion = 0;
            bool done = false;
            CardBoard cardBoard = GetCardBoard(idBoard);
            lock (cardBoard)
            {
                if (command == "CardCreate")
                {
                    string title = context.GetRequestParm("Title");
                    string body = context.GetRequestParm("Body");
                    int x = Convert.ToInt32(context.GetRequestParm("X"));
                    int y = Convert.ToInt32(context.GetRequestParm("Y"));
                    int width = Convert.ToInt32(context.GetRequestParm("Width"));
                    int height = Convert.ToInt32(context.GetRequestParm("Height"));
                    idCard = cardBoard.Card_Create(title, body, x, y, width, height, currentUserName);
                    done = true;
                }
                if (command == "CardMove")
                {
                    idCard = Convert.ToInt32(context.GetRequestParm("IDCard"));
                    int x = Convert.ToInt32(context.GetRequestParm("X"));
                    int y = Convert.ToInt32(context.GetRequestParm("Y"));
                    cardBoard.Card_Move(idCard, x, y, currentUserName);
                    done = true;
                }
                if (command == "CardResize")
                {
                    idCard = Convert.ToInt32(context.GetRequestParm("IDCard"));
                    int width = Convert.ToInt32(context.GetRequestParm("Width"));
                    int height = Convert.ToInt32(context.GetRequestParm("Height"));
                    cardBoard.Card_Resize(idCard, width, height, currentUserName);
                    done = true;
                }
                if (command == "CardEdit")
                {
                    idCard = Convert.ToInt32(context.GetRequestParm("IDCard"));
                    string title = context.GetRequestParm("Title");
                    string body = context.GetRequestParm("Body");
                    cardBoard.Card_Edit(idCard, title, body, currentUserName);
                    done = true;
                }
                if (command == "CardDelete")
                {
                    idCard = Convert.ToInt32(context.GetRequestParm("IDCard"));
                    cardBoard.Card_Delete(idCard, currentUserName);
                    done = true;
                }
                if (command == "RegionCreate")
                {
                    string title = context.GetRequestParm("Title");
                    int x = Convert.ToInt32(context.GetRequestParm("X"));
                    int y = Convert.ToInt32(context.GetRequestParm("Y"));
                    int width = Convert.ToInt32(context.GetRequestParm("Width"));
                    int height = Convert.ToInt32(context.GetRequestParm("Height"));
                    idRegion = cardBoard.Region_Create(title, x, y, width, height, currentUserName);
                    done = true;
                }
                if (command == "RegionMove")
                {
                    idRegion = Convert.ToInt32(context.GetRequestParm("IDRegion"));
                    int x = Convert.ToInt32(context.GetRequestParm("X"));
                    int y = Convert.ToInt32(context.GetRequestParm("Y"));
                    cardBoard.Region_Move(idRegion, x, y, currentUserName);
                    done = true;
                }
                if (command == "RegionResize")
                {
                    idRegion = Convert.ToInt32(context.GetRequestParm("IDRegion"));
                    int width = Convert.ToInt32(context.GetRequestParm("Width"));
                    int height = Convert.ToInt32(context.GetRequestParm("Height"));
                    cardBoard.Region_Resize(idRegion, width, height, currentUserName);
                    done = true;
                }
                if (command == "RegionEdit")
                {
                    idRegion = Convert.ToInt32(context.GetRequestParm("IDRegion"));
                    string title = context.GetRequestParm("Title");
                    cardBoard.Region_Edit(idRegion, title, currentUserName);
                    done = true;
                }
                if (command == "RegionDelete")
                {
                    idRegion = Convert.ToInt32(context.GetRequestParm("IDRegion"));
                    cardBoard.Region_Delete(idRegion, currentUserName);
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

        #endregion Private methods
    }
}