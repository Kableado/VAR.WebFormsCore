using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VAR.Focus.Web.Controls
{
    public class ChatControl : Control, INamingContainer
    {
        #region Declarations

        private string _serviceUrl = "ChatHandler";

        private int _idBoard = 0;

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
            string strCfgName = string.Format("{0}_cfg", this.ClientID);

            _divChatWindow = new Panel { ID = "divChatWindow", CssClass = "divChatWindow" };
            Controls.Add(_divChatWindow);

            _divChatTitleBar = new Panel { ID = "divChatTitleBar", CssClass = "divChatTitleBar" };
            _divChatWindow.Controls.Add(_divChatTitleBar);

            CLabel lblTitle = new CLabel();
            lblTitle.ID = "lblTitle";
            _divChatTitleBar.Controls.Add(lblTitle);

            _divChatContainer = new Panel { ID = "divChatContainer", CssClass = "divChatContainer" };
            _divChatWindow.Controls.Add(_divChatContainer);
            _divChatContainer.Width = _width;
            _divChatContainer.Height = _height;

            var divChat = new Panel { ID = "divChat", CssClass = "divChat" };
            _divChatContainer.Controls.Add(divChat);

            var divChatControls = new Panel { ID = "divChatControls", CssClass = "divChatControls" };
            _divChatContainer.Controls.Add(divChatControls);

            var txtText = new TextBox { ID = "txtText", CssClass = "chatTextBox" };
            txtText.Attributes.Add("autocomplete", "off");
            txtText.Attributes.Add("onkeydown", String.Format("if(event.keyCode==13){{SendChat({0}); return false;}}", strCfgName));
            divChatControls.Controls.Add(txtText);

            var btnSend = new Button { ID = "btnSend", Text = "Send", CssClass = "chatButton" };
            divChatControls.Controls.Add(btnSend);
            btnSend.Attributes.Add("onclick", String.Format("SendChat({0}); return false;", strCfgName));

            StringBuilder sbCfg = new StringBuilder();
            sbCfg.AppendFormat("<script>\n");
            sbCfg.AppendFormat("var {0} = {{\n", strCfgName);
            sbCfg.AppendFormat("  divChatWindow: \"{0}\",\n", _divChatWindow.ClientID);
            sbCfg.AppendFormat("  divChatTitleBar: \"{0}\",\n", _divChatTitleBar.ClientID);
            sbCfg.AppendFormat("  lblTitle: \"{0}\",\n", lblTitle.ClientID);
            sbCfg.AppendFormat("  divChatContainer: \"{0}\",\n", _divChatContainer.ClientID);
            sbCfg.AppendFormat("  divChat: \"{0}\",\n", divChat.ClientID);
            sbCfg.AppendFormat("  txtText: \"{0}\",\n", txtText.ClientID);
            sbCfg.AppendFormat("  btnSend: \"{0}\",\n", btnSend.ClientID);
            sbCfg.AppendFormat("  IDBoard: {0},\n", _idBoard);
            sbCfg.AppendFormat("  UserName: \"{0}\",\n", _userName);
            sbCfg.AppendFormat("  IDMessage: {0},\n", 0);
            sbCfg.AppendFormat("  ServiceUrl: \"{0}\",\n", _serviceUrl);
            sbCfg.AppendFormat("  TimePoolData: {0},\n", _timePoolData);
            sbCfg.AppendFormat("  Texts: {{\n", _serviceUrl);
            sbCfg.AppendFormat("    Chat: \"{0}\",\n", "Chat");
            sbCfg.AppendFormat("    Close: \"{0}\",\n", "Close X");
            sbCfg.AppendFormat("    NewMessages: \"{0}\",\n", "New messages");
            sbCfg.AppendFormat("    Disconnected: \"{0}\",\n", "Disconnected");
            sbCfg.AppendFormat("    StringEmpty: \"\"\n");
            sbCfg.AppendFormat("  }}\n");
            sbCfg.AppendFormat("}};\n");
            sbCfg.AppendFormat("RunChat({0});\n", strCfgName);
            sbCfg.AppendFormat("</script>\n");
            LiteralControl liScript = new LiteralControl(sbCfg.ToString());
            Controls.Add(liScript);
        }

        #endregion
    }
}
