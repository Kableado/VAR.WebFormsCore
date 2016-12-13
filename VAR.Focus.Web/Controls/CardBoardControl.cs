using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAR.Focus.BusinessLogic.JSON;

namespace VAR.Focus.Web.Controls
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

        private int _timeMoveAnimation = 500;

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

        public int TimeMoveAnimation
        {
            get { return _timeMoveAnimation; }
            set { _timeMoveAnimation = value; }
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
            Panel divBoard = new Panel { ID = "divBoard", CssClass = "divBoard" };
            Controls.Add(divBoard);

            string strCfgName = string.Format("{0}_cfg", this.ClientID);
            Dictionary<string, object> cfg = new Dictionary<string, object>
            {
                {"divBoard", divBoard.ClientID},
                {"IDBoard", _idBoard},
                {"UserName", _userName},
                {"IDCardEvent", string.Empty},
                {"ServiceUrl", _serviceUrl},
                {"TimePoolData", _timePoolData},
                {"TimeRefresh", _timeRefresh},
                {"TimeRefreshDisconnected", _timeRefreshDisconnected},
                {"TimeMoveAnimation", _timeMoveAnimation},
                {"Texts", new Dictionary<string, object> {
                    {"Toolbox", "Toolbox"},
                    {"AddCard", "+ Add card"},
                    {"Accept", "Accept"},
                    {"Cancel", "Cancel"},
                    {"ConfirmDelete", "Are you sure to delete?"},
                } },
            };
            JsonWriter jsonWriter = new JsonWriter();
            StringBuilder sbCfg = new StringBuilder();
            sbCfg.AppendFormat("<script>\n");
            sbCfg.AppendFormat("var {0} = {1};\n", strCfgName, jsonWriter.Write(cfg));
            sbCfg.AppendFormat("RunCardBoard({0});\n", strCfgName);
            sbCfg.AppendFormat("</script>\n");
            LiteralControl liScript = new LiteralControl(sbCfg.ToString());
            Controls.Add(liScript);
        }

        #endregion
    }
}
