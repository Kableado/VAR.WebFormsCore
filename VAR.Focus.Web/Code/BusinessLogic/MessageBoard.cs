using System;
using System.Collections.Generic;
using VAR.Focus.Web.Code.Entities;

namespace VAR.Focus.Web.Code.BusinessLogic
{
    public class MessageBoard
    {
        #region Declarations

        private List<Message> _messages = new List<Message>();
        private int _lastIDMessage = 0;

        private string _idMessageBoard = null;

        #endregion

        #region Life cycle

        public MessageBoard(string idMessageBoard)
        {
            _idMessageBoard = idMessageBoard;
            LoadData();
        }

        #endregion 

        #region Public methods

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
                else { break; }
            }
            return listMessages;
        }

        public void Message_Add(string userName, string text)
        {
            lock (_messages)
            {
                _lastIDMessage++;
                Message msg = new Message();
                msg.IDMessage = _lastIDMessage;
                msg.UserName = userName;
                msg.Text = text;
                msg.Date = DateTime.UtcNow;
                _messages.Insert(0, msg);
                SaveData();
            }
        }

        #endregion

        #region Private methods

        #region Persistence

        private const string PersistenceFile = "priv/messageBoard.{0}.json";

        private void LoadData()
        {
            _messages = Persistence.LoadList<Message>(String.Format(PersistenceFile, _idMessageBoard));
            _lastIDMessage = 0;
            if (_messages.Count > 0)
            {
                _lastIDMessage = _messages[0].IDMessage;
            }
        }

        private void SaveData()
        {
            Persistence.SaveList(String.Format(PersistenceFile, _idMessageBoard), _messages);
        }

        #endregion

        #endregion

    }

}
