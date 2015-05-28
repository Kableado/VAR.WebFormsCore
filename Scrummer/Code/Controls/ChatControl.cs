using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Scrummer.Code.Controls
{
    public class ChatControl : Control
    {
        #region Declarations

        private string _serviceUrl = "ChatHandler";

        private int _idBoard = 0;

        private string _userName = string.Empty;

        private Unit _width = new Unit(500, UnitType.Pixel);
        private Unit _height = new Unit(300, UnitType.Pixel);

        private Panel _divChatWindow = null;

        #endregion

        #region Properties

        public string ServiceUrl
        {
            get { return _serviceUrl; }
            set { _serviceUrl = value; }
        }

        public int IDBoard
        {
            get { return _idBoard; }
            set { _idBoard = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public Unit Width
        {
            get { return _width; }
            set
            {
                _width = value;
                if (_divChatWindow != null)
                {
                    _divChatWindow.Width = value;
                }
            }
        }

        public Unit Height
        {
            get { return _height; }
            set
            {
                _height = value;
                if (_divChatWindow != null)
                {
                    _divChatWindow.Height = value;
                }
            }
        }

        #endregion

        #region Control Life cycle

        public ChatControl()
        {
            Init += ChatControl_Init;
        }

        void ChatControl_Init(object sender, EventArgs e)
        {
            InitializeControls();
        }

        #endregion

        #region Private methods

        private void InitializeControls()
        {
            _divChatWindow = new Panel { ID = "divChatWindow", CssClass = "divChatWindow" };
            Controls.Add(_divChatWindow);
            _divChatWindow.Width = _width;
            _divChatWindow.Height = _height;

            var divChat = new Panel { ID = "divChat", CssClass = "divChat" };
            _divChatWindow.Controls.Add(divChat);

            var divChatControls = new Panel { ID = "divChatControls", CssClass = "divChatControls" };
            _divChatWindow.Controls.Add(divChatControls);

            var hidUserName = new HiddenField { ID = "hidUserName", Value = _userName };
            divChatControls.Controls.Add(hidUserName);

            var hidIDMessage = new HiddenField { ID = "hidIDMessage", Value = "0" };
            divChatControls.Controls.Add(hidIDMessage);

            var hidLastUser = new HiddenField { ID = "hidLastUser", Value = "" };
            divChatControls.Controls.Add(hidLastUser);

            var txtText = new TextBox { ID = "txtText", CssClass = "chatTextBox" };
            txtText.Attributes.Add("autocomplete", "off");
            divChatControls.Controls.Add(txtText);

            var btnSend = new Button { ID = "btnSend", Text = "Send", CssClass = "chatButton" };
            divChatControls.Controls.Add(btnSend);
            btnSend.Attributes.Add("onclick", String.Format("SendChat('{0}', '{1}', {2}, '{3}'); return false;",
                _serviceUrl, txtText.ClientID, _idBoard, hidUserName.ClientID));

            LiteralControl litScript = new LiteralControl();
            litScript.Text = String.Format("<script>RunChat('{0}', '{1}', {2}, '{3}', '{4}', '{5}');</script>",
                _serviceUrl, divChat.ClientID, _idBoard, hidIDMessage.ClientID, hidUserName.ClientID, hidLastUser.ClientID);
            Controls.Add(litScript);
        }

        #endregion
    }
}