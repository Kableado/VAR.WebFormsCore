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
        private int _idBoard = 0;

        public int IDBoard
        {
            get { return _idBoard; }
            set { _idBoard = value; }
        }

        public ChatControl()
        {
            Init += ChatControl_Init;
        }

        void ChatControl_Init(object sender, EventArgs e)
        {
            var divChat = new Panel { ID = "divChat", CssClass = "divChat" };
            Controls.Add(divChat);

            var hidUserName = new HiddenField { ID = "hidUserName", Value = "VAR" };
            Controls.Add(hidUserName);

            var hidIDMessage = new HiddenField { ID = "hidIDMessage", Value = "0" };
            Controls.Add(hidIDMessage);

            var txtText = new TextBox { ID = "txtText", CssClass = "chatTextBox" };
            Controls.Add(txtText);

            var btnSend = new Button { ID = "btnSend", Text = "Send", CssClass = "chatButton" };
            Controls.Add(btnSend);
            btnSend.Attributes.Add("onclick", String.Format("SendChat('{0}',{1}, '{2}'); return false;",
                txtText.ClientID, _idBoard, hidUserName.ClientID));


            LiteralControl litScript = new LiteralControl();
            litScript.Text = String.Format("<script>RunChat('{0}',{1},'{2}','{3}');</script>",
                divChat.ClientID, _idBoard, hidIDMessage.ClientID, hidUserName.ClientID);
            Controls.Add(litScript);
        }
    }
}