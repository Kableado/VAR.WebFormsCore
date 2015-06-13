using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrummer.Code.Entities
{
    public interface ICardEvent
    {
        int IDCardEvent { get; set; }
        string EventType { get; set; }
        int IDCard { get; set; }
        string UserName { get; set; }
    }

    public class CardCreateEvent : ICardEvent
    {
        #region ICardEvent

        public int IDCardEvent { get; set; }
        private string _eventType="CardCreate";
        public string EventType { get { return _eventType; } set { _eventType = value; } }
        public int IDCard { get; set; }
        public string UserName { get; set; }

        #endregion

        public string Title { get; set; }
        public string Body { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }

    public class CardMoveEvent : ICardEvent
    {
        #region ICardEvent

        public int IDCardEvent { get; set; }
        private string _eventType = "CardMove";
        public string EventType { get { return _eventType; } set { _eventType = value; } }
        public int IDCard { get; set; }
        public string UserName { get; set; }

        #endregion

        public int X { get; set; }
        public int Y { get; set; }
    }

    public class CardEditEvent : ICardEvent
    {
        #region ICardEvent

        public int IDCardEvent { get; set; }
        private string _eventType = "CardEdit";
        public string EventType { get { return _eventType; } set { _eventType = value; } }
        public int IDCard { get; set; }
        public string UserName { get; set; }

        #endregion

        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class CardDeleteEvent : ICardEvent
    {
        #region ICardEvent

        public int IDCardEvent { get; set; }
        private string _eventType = "CardDelete";
        public string EventType { get { return _eventType; } set { _eventType = value; } }
        public int IDCard { get; set; }
        public string UserName { get; set; }

        #endregion
    }

}