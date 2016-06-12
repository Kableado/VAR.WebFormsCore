using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAR.Focus.Web.Code.JSON;

namespace VAR.Focus.Web.Controls
{
    public class ChatControl : Control, INamingContainer
    {
        #region Declarations

        private string _serviceUrl = "ChatHandler";

        private string _idMessageBoard = null;

        private string _userName = string.Empty;

        private int _timePoolData = 10000;

        private Unit _width = new Unit(500, UnitType.Pixel);
        private Unit _height = new Unit(300, UnitType.Pixel);

        private Panel _divChatWindow = null;
        private Panel _divChatContainer = null;
        private Panel _divChatTitleBar = null;

        #endregion

        #region Properties

        public string ServiceUrl
        {
            get { return _serviceUrl; }
            set { _serviceUrl = value; }
        }

        public string IDMessageBoard
        {
            get { return _idMessageBoard; }
            set { _idMessageBoard = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public int TimePoolData
        {
            get { return _timePoolData; }
            set { _timePoolData = value; }
        }

        public Unit Width
        {
            get { return _width; }
            set
            {
                _width = value;
                if (_divChatContainer != null)
                {
                    _divChatContainer.Width = value;
                }
            }
        }

        public Unit Height
        {
            get { return _height; }
            set
            {
                _height = value;
                if (_divChatContainer != null)
                {
                    _divChatContainer.Height = value;
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
            string strCfgName = string.Format("{0}_cfg", ClientID);

            _divChatWindow = new Panel { ID = "divChatWindow", CssClass = "divChatWindow" };
            Controls.Add(_divChatWindow);

            _divChatTitleBar = new Panel { ID = "divChatTitleBar", CssClass = "divChatTitleBar" };
            _divChatWindow.Controls.Add(_divChatTitleBar);

            CLabel lblTitle = new CLabel();
            lblTitle.ID = "lblTitle";
            _divChatTitleBar.Controls.Add(lblTitle);

            _divChatContainer = new Panel { ID = "divChatContainer", CssClass = "divChatContainer" };
            _divChatWindow.Controls.Add(_divChatContainer);
            _divChatContainer.Width = 0;
            _divChatContainer.Height = 0;

            var divChat = new Panel { ID = "divChat", CssClass = "divChat" };
            _divChatContainer.Controls.Add(divChat);

            var divChatControls = new Panel { ID = "divChatControls", CssClass = "divChatControls" };
            _divChatContainer.Controls.Add(divChatControls);

            var txtText = new TextBox { ID = "txtText", CssClass = "chatTextBox" };
            txtText.Attributes.Add("autocomplete", "off");
            txtText.Attributes.Add("onkeydown", string.Format("if(event.keyCode==13){{SendChat({0}); return false;}}", strCfgName));
            divChatControls.Controls.Add(txtText);

            var btnSend = new Button { ID = "btnSend", Text = "Send", CssClass = "chatButton" };
            divChatControls.Controls.Add(btnSend);
            btnSend.Attributes.Add("onclick", string.Format("SendChat({0}); return false;", strCfgName));
            
            Dictionary<string, object> cfg = new Dictionary<string, object>
            {
                {"divChatWindow", _divChatWindow.ClientID},
                {"divChatTitleBar", _divChatTitleBar.ClientID},
                {"lblTitle", lblTitle.ClientID},
                {"divChatContainer", _divChatContainer.ClientID},
                {"divChatContainerWidth", _width.ToString()},
                {"divChatContainerHeight", _height.ToString()},
                {"divChat", divChat.ClientID},
                {"txtText", txtText.ClientID},
                {"btnSend", btnSend.ClientID},
                {"IDMessageBoard", _idMessageBoard},
                {"UserName", _userName},
                {"IDMessage", 0},
                {"ServiceUrl", _serviceUrl},
                {"TimePoolData", _timePoolData},
                {"Texts", new Dictionary<string, object> {
                    {"Chat", "Chat"},
                    {"Close", "Close X"},
                    {"NewMessages", "New messages"},
                    {"Disconnected", "Disconnected"},
                } },
            };
            JSONWriter jsonWriter = new JSONWriter();
            StringBuilder sbCfg = new StringBuilder();
            sbCfg.AppendFormat("<script>\n");
            sbCfg.AppendFormat("var {0} = {1};\n", strCfgName, jsonWriter.Write(cfg));
            sbCfg.AppendFormat("RunChat({0});\n", strCfgName);
            sbCfg.AppendFormat("</script>\n");
            LiteralControl liScript = new LiteralControl(sbCfg.ToString());
            Controls.Add(liScript);
        }

        #endregion
    }
}
