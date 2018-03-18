using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAR.Json;

namespace VAR.Focus.Web.Controls
{
    public class CTextBox : Control, INamingContainer, IValidableControl
    {
        #region Declarations

        private TextBox _txtContent = new TextBox();

        private HiddenField _hidSize = null;

        private const string CssClassBase = "textbox";
        private string _cssClassExtra = "";

        private bool _allowEmpty = true;

        private string _placeHolder = string.Empty;

        private bool _markedInvalid = false;

        private Control _nextFocusOnEnter = null;

        private bool _keepSize = false;

        #endregion Declarations

        #region Properties

        public string CssClassExtra
        {
            get { return _cssClassExtra; }
            set { _cssClassExtra = value; }
        }

        public bool AllowEmpty
        {
            get { return _allowEmpty; }
            set { _allowEmpty = value; }
        }

        public string PlaceHolder
        {
            get { return _placeHolder; }
            set { _placeHolder = value; }
        }

        public bool MarkedInvalid
        {
            get { return _markedInvalid; }
            set { _markedInvalid = value; }
        }

        public Control NextFocusOnEnter
        {
            get { return _nextFocusOnEnter; }
            set { _nextFocusOnEnter = value; }
        }

        public bool KeepSize
        {
            get { return _keepSize; }
            set { _keepSize = value; }
        }

        public string Text
        {
            get { return _txtContent.Text; }
            set { _txtContent.Text = value; }
        }

        public TextBoxMode TextMode
        {
            get { return _txtContent.TextMode; }
            set { _txtContent.TextMode = value; }
        }

        #endregion Properties

        #region Control life cycle

        public CTextBox()
        {
            Init += CTextBox_Init;
            PreRender += CTextbox_PreRender;
        }

        private void CTextBox_Init(object sender, EventArgs e)
        {
            Controls.Add(_txtContent);

            if (TextMode == TextBoxMode.MultiLine)
            {
                if (_keepSize)
                {
                    _hidSize = new HiddenField();
                    Controls.Add(_hidSize);
                }

                string strCfgName = string.Format("{0}_cfg", this.ClientID);
                Dictionary<string, object> cfg = new Dictionary<string, object>
                {
                    {"txtContent", _txtContent.ClientID},
                    {"hidSize", _hidSize.ClientID},
                    {"keepSize", _keepSize },
                };
                JsonWriter jsonWriter = new JsonWriter();
                StringBuilder sbCfg = new StringBuilder();
                sbCfg.AppendFormat("<script>\n");
                sbCfg.AppendFormat("var {0} = {1};\n", strCfgName, jsonWriter.Write(cfg));
                sbCfg.AppendFormat("CTextBox_Multiline_Init({0});\n", strCfgName);
                sbCfg.AppendFormat("</script>\n");
                LiteralControl liScript = new LiteralControl(sbCfg.ToString());
                Controls.Add(liScript);
            }
        }

        private void CTextbox_PreRender(object sender, EventArgs e)
        {
            _txtContent.CssClass = CssClassBase;
            if (string.IsNullOrEmpty(_cssClassExtra) == false)
            {
                _txtContent.CssClass = string.Format("{0} {1}", CssClassBase, _cssClassExtra);
            }
            if (Page.IsPostBack && (_allowEmpty == false && IsEmpty()) || _markedInvalid)
            {
                _txtContent.CssClass += " textboxInvalid";
            }
            _txtContent.Attributes.Add("onchange", "ElementRemoveClass(this, 'textboxInvalid');");

            if (string.IsNullOrEmpty(_placeHolder) == false)
            {
                _txtContent.Attributes.Add("placeholder", _placeHolder);
            }

            if (_nextFocusOnEnter != null)
            {
                _txtContent.Attributes.Add("onkeydown", string.Format(
                    "if(event.keyCode==13){{document.getElementById('{0}').focus(); return false;}}",
                    _nextFocusOnEnter.ClientID));
            }

            // FIX: The framework deletes textbox values on password mode
            if (_txtContent.TextMode == TextBoxMode.Password)
            {
                _txtContent.Attributes["value"] = _txtContent.Text;
            }
        }

        #endregion Control life cycle

        #region Public methods

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(_txtContent.Text);
        }

        public bool IsValid()
        {
            return _allowEmpty || (string.IsNullOrEmpty(_txtContent.Text) == false);
        }

        public int? GetClientsideHeight()
        {
            if (string.IsNullOrEmpty(_hidSize.Value))
            {
                return null;
            }
            JsonParser jsonParser = new JsonParser();
            Dictionary<string, object> sizeObj = jsonParser.Parse(_hidSize.Value) as Dictionary<string, object>;
            if (sizeObj == null) { return null; }

            if (sizeObj.ContainsKey("height") == false) { return null; }
            return (int)sizeObj["height"];
        }

        public void SetClientsideHeight(int? height)
        {
            if(height == null)
            {
                _hidSize.Value = string.Empty;
                return;
            }
            Dictionary<string, object> sizeObj = null;
            if (string.IsNullOrEmpty(_hidSize.Value) == false)
            {
                JsonParser jsonParser = new JsonParser();
                sizeObj = jsonParser.Parse(_hidSize.Value) as Dictionary<string, object>;
            }
            else
            {
                sizeObj = new Dictionary<string, object> {
                    { "height", height },
                    { "width", null },
                    { "scrollTop", null },
                };
            }
            JsonWriter jsonWriter = new JsonWriter();
            _hidSize.Value = jsonWriter.Write(sizeObj);
        }
        
        #endregion Public methods
    }
}