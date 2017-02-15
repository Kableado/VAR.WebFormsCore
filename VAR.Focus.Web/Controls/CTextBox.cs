﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VAR.Focus.Web.Controls
{
    public class CTextBox : TextBox, IValidableControl
    {
        #region Declarations

        private const string CssClassBase = "textbox";
        private string _cssClassExtra = "";

        private bool _allowEmpty = true;

        private string _placeHolder = string.Empty;

        private bool _markedInvalid = false;

        private Control _nextFocusOnEnter = null;

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

        #endregion Properties

        #region Control life cycle

        public CTextBox()
        {
            PreRender += CTextbox_PreRender;
        }

        private void CTextbox_PreRender(object sender, EventArgs e)
        {
            CssClass = CssClassBase;
            if (string.IsNullOrEmpty(_cssClassExtra) == false)
            {
                CssClass = string.Format("{0} {1}", CssClassBase, _cssClassExtra);
            }
            if (Page.IsPostBack && (_allowEmpty == false && IsEmpty()) || _markedInvalid)
            {
                CssClass += " textboxInvalid";
            }
            Attributes.Add("onchange", "ElementRemoveClass(this, 'textboxInvalid');");

            if (string.IsNullOrEmpty(_placeHolder) == false)
            {
                Attributes.Add("placeholder", _placeHolder);
            }

            if (_nextFocusOnEnter != null)
            {
                this.Attributes.Add("onkeydown", string.Format(
                    "if(event.keyCode==13){{document.getElementById('{0}').focus(); return false;}}",
                    _nextFocusOnEnter.ClientID));
            }

            // FIX: The framework deletes textbox values on password mode
            if (TextMode == TextBoxMode.Password)
            {
                Attributes["value"] = Text;
            }
        }

        #endregion Control life cycle

        #region Public methods

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Text);
        }

        public bool IsValid()
        {
            return _allowEmpty || (string.IsNullOrEmpty(Text) == false);
        }

        #endregion Public methods
    }
}