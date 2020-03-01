using System;
using System.Web.UI.WebControls;
using VAR.Focus.BusinessLogic;
using VAR.Focus.Web.Code;
using VAR.WebForms.Common.Code;
using VAR.WebForms.Common.Controls;
using VAR.WebForms.Common.Pages;

namespace VAR.Focus.Web.Pages
{
    public class FrmLogin : PageCommon
    {
        #region Declarations

        private CTextBox _txtNameEmail = new CTextBox { ID = "txtNameEmail", CssClassExtra = "width150px", AllowEmpty = false };
        private CTextBox _txtPassword = new CTextBox { ID = "txtPassword", CssClassExtra = "width150px", AllowEmpty = false, TextMode = TextBoxMode.Password };
        private CButton _btnLogin = new CButton { ID = "btnLogin" };

        #endregion Declarations

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

        #endregion Page life cycle

        #region UI Events

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (FormUtils.Controls_AreValid(Controls) == false) { return; }

            if (Users.Current.User_Authenticate(_txtNameEmail.Text, _txtPassword.Text) == false)
            {
                _txtPassword.Text = string.Empty;
                return;
            }

            WebSessions.Current.Session_Init(Context, _txtNameEmail.Text);
            Response.Redirect(GlobalConfig.Get().DefaultHandler);
        }

        #endregion UI Events

        #region Private methods

        private void InitializeControls()
        {
            Title = MultiLang.GetLiteral("Login");
            var lblTitle = new CLabel { Text = Title, Tag = "h2" };
            Controls.Add(lblTitle);

            Controls.Add(FormUtils.CreateField(MultiLang.GetLiteral("NameOrMail"), _txtNameEmail));
            _txtNameEmail.NextFocusOnEnter = _txtPassword;
            _txtNameEmail.PlaceHolder = MultiLang.GetLiteral("NameOrMail");

            Controls.Add(FormUtils.CreateField(MultiLang.GetLiteral("Password"), _txtPassword));
            _txtPassword.NextFocusOnEnter = _btnLogin;
            _txtPassword.PlaceHolder = MultiLang.GetLiteral("Password");

            Controls.Add(FormUtils.CreateField(string.Empty, _btnLogin));
            _btnLogin.Text = MultiLang.GetLiteral("Login");
            _btnLogin.Click += btnLogin_Click;

            Controls.Add(FormUtils.CreateField(string.Empty, new HyperLink { Text = MultiLang.GetLiteral("RegisterUser"), NavigateUrl = "FrmRegister" }));
        }

        #endregion Private methods
    }
}