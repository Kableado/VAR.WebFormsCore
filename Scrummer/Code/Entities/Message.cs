using System;

namespace Scrummer.Code.Entities
{
    public class Message
    {
        public int IDMessage { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    };
}
