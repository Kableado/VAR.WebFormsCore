using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scrummer.Controls
{
    public class CardBoardControl : Control, INamingContainer
    {
        #region Declarations

        private string _serviceUrl = "CardBoardHandler";

        private int _idBoard = 0;

        private string _userName = string.Empty;

        private int _timePoolData = 10000;

        private int _timeRefresh = 20;

        private int _timeRefreshDisconnected = 5000;

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

        public int TimeRefresh
        {
            get { return _timeRefresh; }
            set { _timeRefresh = value; }
        }

        public int TimeRefreshDisconnected
        {
            get { return _timeRefreshDisconnected; }
            set { _timeRefreshDisconnected = value; }
        }

        #endregion
        
        #region Control Life cycle

        public CardBoardControl()
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

            Panel divBoard = new Panel { ID = "divBoard", CssClass = "divBoard" };
            Controls.Add(divBoard);

            StringBuilder sbCfg = new StringBuilder();
            sbCfg.AppendFormat("<script>\n");
            sbCfg.AppendFormat("var {0} = {{\n", strCfgName);
            sbCfg.AppendFormat("  divBoard: \"{0}\",\n", divBoard.ClientID);
            sbCfg.AppendFormat("  IDBoard: {0},\n", _idBoard);
            sbCfg.AppendFormat("  UserName: \"{0}\",\n", _userName);
            sbCfg.AppendFormat("  IDCardEvent: \"\",\n");
            sbCfg.AppendFormat("  ServiceUrl: \"{0}\",\n", _serviceUrl);
            sbCfg.AppendFormat("  TimePoolData: {0},\n", _timePoolData);
            sbCfg.AppendFormat("  TimeRefresh: {0},\n", _timeRefresh);
            sbCfg.AppendFormat("  TimeRefreshDisconnected: {0},\n", _timeRefreshDisconnected);
            sbCfg.AppendFormat("  Texts: {{\n");
            sbCfg.AppendFormat("    Toolbox: \"Toolbox\",\n");
            sbCfg.AppendFormat("    AddCard: \"+ Add card\",\n");
            sbCfg.AppendFormat("    Accept: \"Accept\",\n");
            sbCfg.AppendFormat("    Cancel: \"Cancel\",\n");
            sbCfg.AppendFormat("    ConfirmDelete: \"Are you sure to delete?\",\n");
            sbCfg.AppendFormat("    StringEmpty: \"\"\n");
            sbCfg.AppendFormat("  }}\n");
            sbCfg.AppendFormat("}};\n");
            sbCfg.AppendFormat("RunCardBoard({0});\n", strCfgName);
            sbCfg.AppendFormat("</script>\n");
            LiteralControl liScript = new LiteralControl(sbCfg.ToString());
            Controls.Add(liScript);
        }

        #endregion
    }
}