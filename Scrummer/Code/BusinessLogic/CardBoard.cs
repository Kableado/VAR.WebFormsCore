using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scrummer.Code.Entities;

namespace Scrummer.Code.BusinessLogic
{
    public class CardBoard
    {
        #region Declarations

        private List<Card> _cards = new List<Card>();
        private int _lastIDCard = 0;

        private List<ICardEvent> _cardEvents = new List<ICardEvent>();
        private int _lastIDCardEvent = 0;

        private int _idBoard = 0;

        #endregion

        #region Life cycle

        public CardBoard(int idBoard)
        {
            _idBoard = idBoard;
            LoadData();
        }

        #endregion

        #region Public methods

        public List<Card> Cards_Status()
        {
            List<Card> activeCards=new List<Card>();
            foreach (Card card in _cards)
            {
                if (card.Active)
                {
                    activeCards.Add(card);
                }
            }
            return activeCards;
        }

        public List<ICardEvent> Cards_GetEventList(int idCardEvent)
        {
            List<ICardEvent> listEvents = new List<ICardEvent>();
            for (int i = 0, n = _cardEvents.Count; i < n; i++)
            {
                ICardEvent cardEvent = _cardEvents[i];
                if (cardEvent.IDCardEvent > idCardEvent)
                {
                    listEvents.Insert(0, cardEvent);
                }
                else { break; }
            }
            return listEvents;
        }

        public int GetLastIDCardEvent()
        {
            return _lastIDCardEvent;
        }

        public int GetLastIDCard()
        {
            return _lastIDCard;
        }

        public int Card_Create(string title, string body, int x, int y, string currentUserName)
        {
            Card card;
            lock (_cards)
            {
                // Create card
                _lastIDCard++;
                card = new Card()
                {
                    IDCard = _lastIDCard,
                    Title = title,
                    Body = body,
                    X = x,
                    Y = y,
                    Active = true,
                    CreatedBy = currentUserName,
                    ModifiedBy = currentUserName,
                };
                _cards.Add(card);

                // Create event
                _lastIDCardEvent++;
                CardCreateEvent cardCreateEvent = new CardCreateEvent()
                {
                    IDCardEvent = _lastIDCardEvent,
                    IDCard = card.IDCard,
                    UserName = currentUserName,
                    Title = card.Title,
                    Body = card.Body,
                    X = card.X,
                    Y = card.Y,
                };
                _cardEvents.Insert(0, cardCreateEvent);

                SaveData();
            }
            return card.IDCard;
        }

        public bool Card_Move(int idCard, int x, int y, string currentUserName)
        {
            lock (_cards)
            {
                // Move card
                Card card = GetByID(idCard);
                if (card == null) { return false; }
                card.X = x;
                card.Y = y;
                card.ModifiedBy = currentUserName;

                // Create event
                _lastIDCardEvent++;
                CardMoveEvent cardMoveEvent = new CardMoveEvent()
                {
                    IDCardEvent = _lastIDCardEvent,
                    IDCard = card.IDCard,
                    UserName = currentUserName,
                    X = card.X,
                    Y = card.Y,
                };
                _cardEvents.Insert(0, cardMoveEvent);

                SaveData();
            }
            return true;
        }

        public bool Card_Edit(int idCard, string title, string body, string currentUserName)
        {
            lock (_cards)
            {
                // Edit card
                Card card = GetByID(idCard);
                if (card == null) { return false; }
                card.Title = title;
                card.Body = body;
                card.ModifiedBy = currentUserName;

                // Create event
                _lastIDCardEvent++;
                CardEditEvent cardEditEvent = new CardEditEvent()
                {
                    IDCardEvent = _lastIDCardEvent,
                    IDCard = card.IDCard,
                    UserName = currentUserName,
                    Title = card.Title,
                    Body = card.Body,
                };
                _cardEvents.Insert(0, cardEditEvent);

                SaveData();
            }
            return true;
        }

        public bool Card_Delete(int idCard, string currentUserName)
        {
            lock (_cards)
            {
                // Delete card
                Card card = GetByID(idCard);
                if (card == null) { return false; }
                card.Active = false;
                card.ModifiedBy = currentUserName;
                
                // Create event
                _lastIDCardEvent++;
                CardDeleteEvent cardDeleteEvent = new CardDeleteEvent()
                {
                    IDCardEvent = _lastIDCardEvent,
                    IDCard = card.IDCard,
                    UserName = currentUserName,
                };
                _cardEvents.Insert(0, cardDeleteEvent);

                SaveData();
            }
            return true;
        }

        public static List<ICardEvent> ConvertCardsToEvents(List<Card> listCards, int lastIDCardEvent)
        {
            List<ICardEvent> listEvents = new List<ICardEvent>();
            foreach (Card card in listCards)
            {
                var evt = new CardCreateEvent()
                {
                    IDCardEvent = lastIDCardEvent,
                    IDCard = card.IDCard,
                    UserName = card.ModifiedBy,
                    Title = card.Title,
                    Body = card.Body,
                    X = card.X,
                    Y = card.Y,
                };
                listEvents.Add(evt);
            }
            return listEvents;
        }

        #endregion

        #region Private methods

        private Card GetByID(int idCard)
        {
            foreach (Card card in _cards)
            {
                if (card.IDCard == idCard)
                {
                    return card;
                }
            }
            return null;
        }

        #region Persistence

        private const string CardsPersistenceFile = "priv/cardBoard.{0}.json";
        private const string EventsPersistenceFile = "priv/cardEvents.{0}.json";

        private void LoadData()
        {
            _cards = Persistence.LoadList<Card>(String.Format(CardsPersistenceFile, _idBoard));
            _lastIDCard = 0;
            foreach (Card card in _cards)
            {
                if (card.IDCard > _lastIDCard)
                {
                    _lastIDCard = card.IDCard;
                }
            }

            _cardEvents = Persistence.LoadList<ICardEvent>(String.Format(EventsPersistenceFile, _idBoard), 
                new List<Type> { 
                    typeof(CardCreateEvent),
                    typeof(CardMoveEvent),  
                    typeof(CardEditEvent), 
                    typeof(CardDeleteEvent),
                });
            _lastIDCardEvent = 0;
            if (_cardEvents.Count > 0)
            {
                _lastIDCardEvent = _cardEvents[0].IDCardEvent;
            }
        }

        private void SaveData()
        {
            Persistence.SaveList(String.Format(CardsPersistenceFile, _idBoard), _cards);
            Persistence.SaveList(String.Format(EventsPersistenceFile, _idBoard), _cardEvents);
        }

        #endregion

        #endregion

    }
}