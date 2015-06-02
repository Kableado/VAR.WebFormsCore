﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scrummer.Code.Controls
{
    public class CTextBox : TextBox, IValidableControl
    {
        #region Declarations

        private const string CssClassBase = "textbox";
        private string _cssClassExtra = "";

        private bool _allowEmpty = true;

        private string _placeHolder = string.Empty;

        #endregion

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
        
        #endregion

        #region Control life cycle

        public CTextBox()
        {
            PreRender += CTextbox_PreRender;
        }

        void CTextbox_PreRender(object sender, EventArgs e)
        {
            CssClass = CssClassBase;
            if (string.IsNullOrEmpty(_cssClassExtra) == false)
            {
                CssClass = String.Format("{0} {1}", CssClassBase, _cssClassExtra);
            }
            if (Page.IsPostBack && _allowEmpty == false && IsEmpty())
            {
                CssClass += " textboxEmpty";
            }
            Attributes.Add("onchange", "ElementRemoveClass(this, 'textboxEmpty');");

            if (string.IsNullOrEmpty(_placeHolder) == false)
            {
                Attributes.Add("placeholder", _placeHolder);
            }

            // FIX: The framework deletes textbox values on password mode
            if (TextMode == TextBoxMode.Password)
            {
                Attributes["value"] = Text;
            }
        }

        #endregion

        #region Public methods

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Text);
        }

        public bool IsValid()
        {
            return _allowEmpty || (string.IsNullOrEmpty(Text) == false);
        }

        #endregion

    }
}