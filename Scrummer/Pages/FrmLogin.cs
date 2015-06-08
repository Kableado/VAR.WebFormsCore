using System;
using System.Web.UI.WebControls;
using Scrummer.Code.BusinessLogic;
using Scrummer.Controls;

namespace Scrummer.Pages
{
    public class FrmLogin : PageCommon
    {
        #region Declarations

        private CTextBox _txtNameEmail = new CTextBox { ID = "txtNameEmail", CssClassExtra = "width150px", AllowEmpty = false };
        private CTextBox _txtPassword = new CTextBox { ID = "txtPassword", CssClassExtra = "width150px", AllowEmpty = false, TextMode = TextBoxMode.Password };
        private CButton _btnLogin = new CButton { ID = "btnLogin"};

        #endregion

        #region Page life cycle

        public FrmLogin()
        {
            MustBeAutenticated = false;
            Init += FrmLogin_Init;
        }

        private void FrmLogin_Init(object sender, EventArgs e)
        {
            InitializeControls();
        }

        #endregion

        #region UI Events

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (FormUtils.Controls_AreValid(Controls) == false) { return; }

            if (Users.Current.User_Authenticate(_txtNameEmail.Text, _txtPassword.Text) == false)
            {
                _txtPassword.Text = string.Empty;
                return;
            }

            Sessions.Current.Session_Init(Context, _txtNameEmail.Text);
            Response.Redirect(".");
        }

        #endregion

        #region Private methods

        private void InitializeControls()
        {
            Title = "Login";
            var lblTitle = new CLabel { Text = "Login", Tag = "h2" };
            Controls.Add(lblTitle);

            Controls.Add(FormUtils.CreateField("Name/Mail", _txtNameEmail));
            _txtNameEmail.PlaceHolder = "Name/Mail";

            Controls.Add(FormUtils.CreateField("Password", _txtPassword));
            _txtPassword.PlaceHolder = "Password";

            Controls.Add(FormUtils.CreateField(String.Empty, _btnLogin));
            _btnLogin.Text = "Login";
            _btnLogin.Click += btnLogin_Click;

            Controls.Add(FormUtils.CreateField(String.Empty, new HyperLink { Text = "Register user", NavigateUrl = "FrmRegister" }));

            _txtNameEmail.Attributes.Add("onkeydown", String.Format(
                "if(event.keyCode==13){{document.getElementById('{0}').focus(); return false;}}", _txtPassword.ClientID));
            _txtPassword.Attributes.Add("onkeydown", String.Format(
                "if(event.keyCode==13){{document.getElementById('{0}').focus(); return false;}}", _btnLogin.ClientID));
        }

        #endregion
    }
}