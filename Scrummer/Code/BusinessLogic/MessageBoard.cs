using System;
using System.Collections.Generic;
using Scrummer.Code.Entities;

namespace Scrummer.Code.BusinessLogic
{
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

}